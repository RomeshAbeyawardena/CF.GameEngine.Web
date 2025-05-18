using CF.Identity.Api.Features.AccessTokens;
using CF.Identity.Api.Middleware;

namespace CF.Identity.Api.Extensions;

public static class AuthMiddlewareExtensions
{
    public static bool TryGetAccessToken(this HttpContext ctx, out AccessTokenDto? accessToken)
    {
        bool isSuccess;
        accessToken = (isSuccess = ctx.Items.TryGetValue(AuthMiddleware.TokenItemKey, out var value)) 
            ? (value as AccessTokenDto) : null;

        return isSuccess;
    }
    public static bool TryGetAuthenticatedClient(this HttpContext context, out AuthenticatedClient? client)
    {
        bool isSuccess;
        client = (isSuccess = context.Items.TryGetValue(AuthMiddleware.ClientItemKey, out var result))
             ? result as AuthenticatedClient
             : null;
        return isSuccess;
    }

    public static bool IsUserAuthenticated(this HttpContext context, out (AccessTokenDto?, AuthenticatedClient?) state)
    {
        AuthenticatedClient? client = null;
        AccessTokenDto? accessToken = null;
        var result = context.TryGetAuthenticatedClient(out client)
            && context.TryGetAccessToken(out accessToken);

        state = (accessToken, client);

        return result;
    }
}
