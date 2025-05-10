using CF.Identity.Api.Features.AccessToken.Get;
using CF.Identity.Api.Features.TokenExchange;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CF.Identity.Api.Features.Introspect;

public class IntrospectQueryHandler(IMediator mediator, IClientCredentialHasher clientCredentialHasher, TimeProvider timeProvider, JwtSettings jwtSettings) 
    : IUnitRequestHandler<IntrospectQuery, IntrospectResponse>
{
    public async Task<IUnitResult<IntrospectResponse>> Handle(IntrospectQuery request, CancellationToken cancellationToken)
    {
        var hashedToken = clientCredentialHasher.Hash(request.Token, request.Client);

        var utcNow = timeProvider.GetUtcNow();
        var foundApiToken = await mediator.Send(new FindAccessTokenQuery(hashedToken, request.Client.Id, ValidFrom: utcNow, ValidTo: utcNow), cancellationToken);

        if (!foundApiToken.HasValue || foundApiToken.Result is null)
        {
            return new UnitResult(new UnauthorizedAccessException("Token not found")).As<IntrospectResponse>();
        }
        
        var latestToken = foundApiToken.Result.OrderByDescending(x => x.ValidFrom).ThenByDescending(x => x.ValidTo).FirstOrDefault();

        if (latestToken is null)
        {
            return new UnitResult(new UnauthorizedAccessException("Token not found")).As<IntrospectResponse>();
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey ?? throw new ArgumentException(nameof(jwtSettings))));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var result = await new JwtSecurityTokenHandler().ValidateTokenAsync(latestToken.AccessToken, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        });

        if(!result.IsValid)
        {
            return new UnitResult(new UnauthorizedAccessException("Token is not valid")).As<IntrospectResponse>();
        }

        string scope = string.Empty;
        if (result.Claims.TryGetValue("scope", out var scopeClaim))
        {
            scope = scopeClaim?.ToString() ?? throw new NullReferenceException("scope is invalid");
        }

        return new UnitResult<IntrospectResponse>(new(
            Active: true,
            ClientId: latestToken.ClientId.ToString(),
            Scope: scope,
            Exp: latestToken.ValidTo?.ToString("o") ?? string.Empty,
            Aud: jwtSettings.Audience ?? string.Empty,
            Iss: jwtSettings.Issuer ?? string.Empty
        ));
    }
}
