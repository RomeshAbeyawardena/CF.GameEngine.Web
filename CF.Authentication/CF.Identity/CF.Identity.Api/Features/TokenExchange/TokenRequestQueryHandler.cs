using CF.Identity.Api.Features.AccessToken;
using CF.Identity.Api.Features.Client;
using CF.Identity.Api.Features.Client.Get;
using CF.Identity.Api.Features.Scope.Get;
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

public class TokenRequestQueryHandler(JwtSettings jwtSettings, IMediator mediator, IClientCredentialHasher clientCredentialHasher,
    TimeProvider timeProvider, RandomNumberGenerator randomNumberGenerator) : IUnitRequestHandler<TokenRequestQuery, TokenResponse>
{
    private string GenerateJwt(ClientDetailResponse client, string scope)
    {
        //TODO come from configuration shared with consumer. /jwks.json
        var claims = new[]
        {
            new Claim("sub", client.Reference),
            new Claim("scope", scope),
            new Claim("client_id", client.Reference)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey ?? throw new ArgumentException(nameof(jwtSettings))));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateSecureRandomBase64(RandomNumberGenerator rng, int sizeInBytes)
    {
        var buffer = new byte[sizeInBytes];
        rng.GetNonZeroBytes(buffer);
        return Convert.ToBase64String(buffer);
    }

    public async Task<IUnitResult<TokenResponse>> Handle(TokenRequestQuery request, CancellationToken cancellationToken)
    {
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
            || utcNow < clientDetail.ValidFrom
            || utcNow > clientDetail.ValidTo
            || clientDetail.SuspendedTimestampUtc.HasValue)
        {
            return new UnitResult(new UnauthorizedAccessException("Invalid client secret")).As<TokenResponse>();
        }

        if (string.IsNullOrWhiteSpace(request.TokenRequest.Scope))
        {
            return new UnitResult(new UnauthorizedAccessException("Scope not provided")).As<TokenResponse>();
        }

        var requestedScopes = request.TokenRequest.Scope.Split();
        var scopes = await mediator.Send(new FindScopeQuery(clientDetail.Id, Keys: requestedScopes), cancellationToken);

        if (!scopes.HasValue || !requestedScopes.All(y => scopes.Result!.Any(x => x.Key.Equals(y, StringComparison.InvariantCultureIgnoreCase))))
        {
            return new UnitResult(new Exception("Invalid scope requested")).As<TokenResponse>();
        }

        var accessToken = GenerateJwt(clientDetail, request.TokenRequest.Scope);

        var referenceToken = GenerateSecureRandomBase64(randomNumberGenerator, 32);
        referenceToken = clientCredentialHasher.Hash(referenceToken, clientDetail);

        var refreshToken = GenerateSecureRandomBase64(randomNumberGenerator, 16);
        var hashedRefreshToken = clientCredentialHasher.Hash(refreshToken, clientDetail);

        await mediator.Send(new UpsertAccessTokenCommand(new AccessTokenDto
        {
            Type = request.TokenRequest.GrantType,
            ReferenceToken = referenceToken,
            RefreshToken = hashedRefreshToken,
            AccessToken = accessToken,
            ClientId = clientDetail.Id,
            ValidFrom = utcNow,
            ValidTo = utcNow.AddHours(1)
        }), cancellationToken);

        //TODO: Generate token
        var result = new UnitResult<TokenResponse>(new TokenResponse(referenceToken,
                "Bearer",
                "3600",
                refreshToken,
                request.TokenRequest.Scope
            ));

        if (!string.IsNullOrWhiteSpace(request.TokenRequest.RedirectUri))
        {
            result.AddMeta("redirectUri", request.TokenRequest.RedirectUri);
        }

        return result;
    }
}