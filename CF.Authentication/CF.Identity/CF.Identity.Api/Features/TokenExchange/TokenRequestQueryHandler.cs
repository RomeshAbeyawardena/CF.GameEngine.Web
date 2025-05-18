using CF.Identity.Api.Features.AccessTokens;
using CF.Identity.Api.Features.Clients;
using CF.Identity.Api.Features.Clients.Get;
using CF.Identity.Api.Features.Scopes.Get;
using CF.Identity.Api.Features.User.Get;
using CF.Identity.Infrastructure;
using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;
using MediatR;
using System.Security.Cryptography;


namespace CF.Identity.Api.Features.TokenExchange;

public class TokenRequestQueryHandler(IJwtSettings jwtSettings, IMediator mediator, IUserCredentialProtectionProvider userCredentialProtectionProvider, 
    IClientCredentialHasher clientCredentialHasher, TimeProvider timeProvider, RandomNumberGenerator randomNumberGenerator) : IUnitRequestHandler<TokenRequestQuery, TokenResponse>
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

        var clientResult = await mediator.Send(new FindClientQuery(request.TokenRequest.ClientId, dateRange.FromValue, dateRange.ToValue), cancellationToken);

        var clientDetail = clientResult.GetOneOrDefault();

        if (clientDetail is null)
        {
            return new UnitResult(new UnauthorizedAccessException("Client not found")).As<TokenResponse>();
        }

        if (!clientCredentialHasher.Verify(request.TokenRequest.ClientSecret, clientDetail))
        {
            return new UnitResult(new UnauthorizedAccessException("Invalid client secret")).As<TokenResponse>();
        }

        if (string.IsNullOrWhiteSpace(request.TokenRequest.Scope))
        {
            return new UnitResult(new UnauthorizedAccessException("Scope not provided")).As<TokenResponse>();
        }

        var requestedScopes = request.TokenRequest.Scope.Split();
        var scopesResponse = await mediator.Send(new FindScopeQuery(clientDetail.Id, Keys: requestedScopes), cancellationToken);
        var scopes = scopesResponse.AsList();
        if (!requestedScopes.All(y => scopes.Any(x => x.Key.Equals(y, StringComparison.InvariantCultureIgnoreCase))))
        {
            return new UnitResult(new Exception("Invalid scope requested")).As<TokenResponse>();
        }

        var isSystemUser = string.IsNullOrEmpty(request.TokenRequest.Username);

        string? username = null;
        if (!isSystemUser)
        {
            username = userCredentialProtectionProvider.HashUsingHmac(clientDetail, request.TokenRequest.Username);
        }

        var userResult = await mediator.Send(new FindUsersQuery(clientDetail.Id, Username: username, IsSystem: isSystemUser), cancellationToken);

        var systemUser = userResult.GetOneOrDefault();

        if (systemUser is null)
        {
            var prefix = isSystemUser ? "System u" : "U";
            return new UnitResult(new UnauthorizedAccessException($"{prefix}ser not found")).As<TokenResponse>();
        }

        var accessToken = GenerateJwt(clientDetail, request.TokenRequest.Scope);

        var referenceToken = JwtHelper.GenerateSecureRandomBase64(randomNumberGenerator, 32);
        var hashedReferenceToken = clientCredentialHasher.Hash(referenceToken, clientDetail);

        var refreshToken = JwtHelper.GenerateSecureRandomBase64(randomNumberGenerator, 16);
        var hashedRefreshToken = clientCredentialHasher.Hash(refreshToken, clientDetail);

        await mediator.Send(new UpsertAccessTokenCommand(new AccessTokenDto
        {
            Type = request.TokenRequest.GrantType,
            ReferenceToken = hashedReferenceToken,
            RefreshToken = hashedRefreshToken,
            AccessToken = accessToken,
            ClientId = clientDetail.Id,
            ValidFrom = utcNow,
            ValidTo = utcNow.AddHours(1),
            UserId = systemUser.Id,
        }), cancellationToken);

        //TODO: Generate token
        var result = new UnitResult<TokenResponse>(new TokenResponse(referenceToken,
                "Bearer",
                TimeSpan.FromHours(1).Seconds.ToString(),
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