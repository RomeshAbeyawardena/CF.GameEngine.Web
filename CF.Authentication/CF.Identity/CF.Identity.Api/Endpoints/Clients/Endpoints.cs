using CF.Identity.Api.Endpoints.Clients.Post;

namespace CF.Identity.Api.Endpoints.Clients;

public static class Endpoints
{
    public const string BaseUrl = "api/client";
    public const string GetClient = nameof(GetClient);
    public const string Tag = "Clients";
    public static IEndpointRouteBuilder AddClientEndpoints(this IEndpointRouteBuilder builder)
    {
        return builder
            .AddPostClientEndpoint();
    }
}
