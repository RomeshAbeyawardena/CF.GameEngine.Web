using CF.Identity.Api.Features.AccessTokens;
using CF.Identity.Api.Middleware;

namespace CF.Identity.Api.Extensions;

public static class AuthMiddlewareExtensions
{
    public static AccessTokenDto? GetAccessToken(this HttpContext ctx) =>
        ctx.Items.TryGetValue(AuthMiddleware.TokenItemKey, out var value) ? value as AccessTokenDto : null;

    public static AuthenticatedClient? GetAuthenticatedClient(this HttpContext context)
        => context.Items.TryGetValue(AuthMiddleware.ClientItemKey, out var result)
            ? result as AuthenticatedClient
            : null;
}
