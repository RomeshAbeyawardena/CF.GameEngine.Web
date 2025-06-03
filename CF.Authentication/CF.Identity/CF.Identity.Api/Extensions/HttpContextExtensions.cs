namespace CF.Identity.Api.Extensions;

public static class HttpContextExtensions
{
    public static AuthenticatedClient? GetAuthenticatedClient(this HttpContext context)
    {
        if (context.Items.TryGetValue(nameof(AuthenticatedClient), out var authClient) 
            && authClient is AuthenticatedClient authenticatedClient)
        {
            return authenticatedClient;
        }

        return null;
    }
}
