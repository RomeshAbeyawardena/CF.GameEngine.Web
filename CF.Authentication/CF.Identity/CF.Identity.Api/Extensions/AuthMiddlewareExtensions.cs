using CF.Identity.Api.Middleware;

namespace CF.Identity.Api.Extensions;

public static class AuthMiddlewareExtensions
{
    public static AuthenticatedClient? GetAuthenticatedClient(this HttpContext context)
        => context.Items.TryGetValue("Client", out var result)
            ? result as AuthenticatedClient
            : null;
}
