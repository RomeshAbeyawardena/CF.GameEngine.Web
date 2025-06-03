using CF.Identity.Api.Extensions;
using CF.Identity.Api.Features.AccessTokens;
using CF.Identity.Api.Features.AccessTokens.Delete;
using CF.Identity.Api.Features.AccessTokens.Get;
using CF.Identity.Api.Features.Clients;
using CF.Identity.Api.Features.Clients.Get;
using CF.Identity.Api.Features.Scopes.Get;
using CF.Identity.Api.Features.User.Get;
using CF.Identity.Infrastructure;
using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.PII;
using CF.Identity.Infrastructure.SqlServer.SPA;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;
using MediatR;
using System.Security.Cryptography;


namespace CF.Identity.Api.Features.TokenExchange;

public class TokenRequestQueryHandler(IJwtSettings jwtSettings, IMediator mediator, IClientProtection clientProtection, IHttpContextAccessor httpContextAccessor,
    TimeProvider timeProvider, RandomNumberGenerator randomNumberGenerator) : IUnitRequestHandler<TokenRequestQuery, TokenResponse>
{
    private string GenerateJwt(ClientDetailResponse client, string scope)
    {
        return JwtHelper.GenerateJwt(client, scope, jwtSettings);
    }

    public async Task<IUnitResult<TokenResponse>> Handle(TokenRequestQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.TokenRequest.GrantType)
            || !request.TokenRequest.GrantType.Equals("client_credentials", StringComparison.InvariantCultureIgnoreCase))
        {
            return new UnitResult(new NotSupportedException("Grant type not supported")).As<TokenResponse>();
        }

        var utcNow = timeProvider.GetUtcNow();
        var dateRange = DateTimeOffsetRange.GetValidatyDateRange(utcNow);

        var authenticatedClient = httpContextAccessor.HttpContext?.GetAuthenticatedClient();

        var clientId = request.TokenRequest.ClientId;
        
        //we can only help with the client ID, the client secret is not available in its plaintext value as its thrown away upon use
        if (authenticatedClient is not null)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                clientId = authenticatedClient.ClientDetails.Reference;
            }
        }

        var clientResult = await mediator.Send(new FindClientQuery(clientId, dateRange.FromValue, dateRange.ToValue, Bypass: true), cancellationToken);

        var clientDetail = clientResult.GetOneOrDefault();

        if (clientDetail is null)
        {
            return new UnitResult(new UnauthorizedAccessException("Client not found")).As<TokenResponse>();
        }

        if (string.IsNullOrEmpty(request.TokenRequest.ClientSecret) 
            || !clientProtection.VerifySecret(clientDetail, request.TokenRequest.ClientSecret))
        {
            return new UnitResult(new UnauthorizedAccessException("Invalid client secret")).As<TokenResponse>();
        }

        if (string.IsNullOrWhiteSpace(request.TokenRequest.Scope))
        {
            return new UnitResult(new UnauthorizedAccessException("Scope not provided")).As<TokenResponse>();
        }

        var isSystemUser = string.IsNullOrEmpty(request.TokenRequest.Username);

        var userResult = await mediator.Send(new FindUsersQuery(clientDetail.Id, Username: request.TokenRequest.Username, IsSystem: isSystemUser, Bypass: true), cancellationToken);

        var systemUser = userResult.GetOneOrDefault();

        if (systemUser is null)
        {
            var prefix = isSystemUser ? "System u" : "U";
            return new UnitResult(new UnauthorizedAccessException($"{prefix}ser not found")).As<TokenResponse>();
        }

        var requestedScopes = request.TokenRequest.Scope.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var scopesResponse = await mediator.Send(new FindScopesQuery(clientDetail.Id, Keys: requestedScopes, Bypass: true), cancellationToken);
        var scopes = scopesResponse.AsList();

        if (!requestedScopes.All(y => scopes.Any(x => x.Key.Equals(y, StringComparison.InvariantCultureIgnoreCase))))
        {
            return new UnitResult(new UnauthorizedAccessException("Invalid scope requested")).As<TokenResponse>();
        }

        var existingAccessTokens = await mediator.Send(new FindAccessTokenQuery(ClientId: clientDetail.Id, UserId: systemUser.Id,
            Type: request.TokenRequest.GrantType, ValidFrom: dateRange.FromValue, ValidTo: dateRange.ToValue,
            Bypass: true), cancellationToken);

        if (existingAccessTokens.HasValue)
        {
            await mediator.Send(new BulkExpireAccessTokenCommand(existingAccessTokens.Result.Select(x => x.Id), 
                "Replaced by new token", "System", true), cancellationToken);
        }

        var accessToken = GenerateJwt(clientDetail, request.TokenRequest.Scope);

        var referenceToken = JwtHelper.GenerateSecureRandomBase64(randomNumberGenerator, 32);
        
        var refreshToken = JwtHelper.GenerateSecureRandomBase64(randomNumberGenerator, 16);
        
        await mediator.Send(new UpsertAccessTokenCommand(new AccessTokenDto
        {
            Type = request.TokenRequest.GrantType,
            ReferenceToken = referenceToken,
            RefreshToken = refreshToken,
            AccessToken = accessToken,
            ClientId = clientDetail.Id,
            ValidFrom = utcNow,
            ValidTo = utcNow.AddHours(1),
            UserId = systemUser.Id,
        }, true), cancellationToken);

        
        var result = new UnitResult<TokenResponse>(new TokenResponse(referenceToken,
                "Bearer",
                Convert.ToInt32(TimeSpan.FromHours(1).TotalSeconds),
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