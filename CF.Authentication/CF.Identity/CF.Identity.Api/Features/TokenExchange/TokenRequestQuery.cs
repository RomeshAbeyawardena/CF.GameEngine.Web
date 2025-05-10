using CF.Identity.Api.Features.AccessToken;
using CF.Identity.Api.Features.Client;
using CF.Identity.Api.Features.Client.Get;
using CF.Identity.Infrastructure;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CF.Identity.Api.Features.TokenExchange;

public record TokenRequest(
    string GrantType,
    string ClientId,
    string ClientSecret,
    string Scope,
    string RedirectUri
);

public record TokenResponse(
    string AccessToken,
    string TokenType,
    string ExpiresIn,
    string RefreshToken,
    string Scope
);

public record TokenRequestQuery(TokenRequest TokenRequest) : IUnitRequest<TokenResponse>;

public record JwtSettings(string Issuer, string Audience, string SigningKey);

public class TokenRequestQueryHandler(IMediator mediator, IClientCredentialHasher clientCredentialHasher, 
    TimeProvider timeProvider, RandomNumberGenerator randomNumberGenerator) : IUnitRequestHandler<TokenRequestQuery, TokenResponse>
{
    private static string GenerateJwt(ClientDetailResponse client, string scope)
    {
        //TODO come from configuration shared with consumer. /jwks.json
        var _jwtSettings = new JwtSettings("", "", "");
        var claims = new[]
        {
        new Claim("sub", client.Reference),
        new Claim("scope", scope),
        new Claim("client_id", client.Reference)
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public async Task<IUnitResult<TokenResponse>> Handle(TokenRequestQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        
        if (string.IsNullOrWhiteSpace(request.TokenRequest.GrantType) 
            || !request.TokenRequest.GrantType.Equals("client_credentials", StringComparison.InvariantCultureIgnoreCase))
        {
            return new UnitResult(new NotSupportedException("Grant type not supported")).As<TokenResponse>();
        }

        var clientResult = await mediator.Send(new FindClientQuery(request.TokenRequest.ClientId), cancellationToken);

        var clientDetail = clientResult.Result?.FirstOrDefault();

        if (!clientResult.IsSuccess || clientDetail is null)
        {
            return new UnitResult(new UnauthorizedAccessException("Client not found")).As<TokenResponse>();
        }

        var utcNow = timeProvider.GetUtcNow();
        if (!clientCredentialHasher.Verify(request.TokenRequest.ClientSecret, clientDetail)
            || clientDetail.ValidFrom < utcNow 
            || clientDetail.ValidTo > utcNow
            || clientDetail.SuspendedTimestampUtc.HasValue)
        {
            return new UnitResult(new UnauthorizedAccessException("Invalid client secret")).As<TokenResponse>();
        }

        var accessToken = GenerateJwt(clientDetail, request.TokenRequest.Scope);

        var referenceToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        referenceToken = clientCredentialHasher.Hash(referenceToken, clientDetail);

        await mediator.Send(new UpsertAccessTokenCommand(new AccessTokenDto
        {
            Type = request.TokenRequest.GrantType,
            ReferenceToken = referenceToken,
            AccessToken = accessToken,
            ClientId = clientDetail.Id,
            ValidFrom = utcNow,
            ValidTo = utcNow.AddHours(1)
        }), cancellationToken);
        
        //TODO: Generate token
        var result = new UnitResult<TokenResponse>(new TokenResponse(referenceToken,
                "Bearer",
                "3600",
                "refresh_token",
                request.TokenRequest.Scope
            ));

        if(!string.IsNullOrWhiteSpace(request.TokenRequest.RedirectUri))
        {
            result.AddMeta("redirectUri", request.TokenRequest.RedirectUri);
        }

        return result;
    }
}