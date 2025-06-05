using CF.Identity.Api.Endpoints.Scopes.Get;
using CF.Identity.Api.Endpoints.Scopes.Post;

namespace CF.Identity.Api.Endpoints.Scopes;

public static class Endpoints
{
    public const string BaseUrl = "api/scope";
    public const string GetScope = nameof(GetScope);
    public static IEndpointRouteBuilder AddScopeEndpoints(this IEndpointRouteBuilder builder)
    {
        return builder
            .AddGetScopesEndpoint()
            .AddPostScopeEndpoint();
    }
}
