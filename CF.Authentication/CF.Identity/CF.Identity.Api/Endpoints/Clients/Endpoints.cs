using CF.Identity.Api.Endpoints.Clients.Post;

namespace CF.Identity.Api.Endpoints.Clients;

public static class Endpoints
{
    public const string Url = "api/client";

    public static IEndpointRouteBuilder AddClientEndpoints(this IEndpointRouteBuilder builder)
    {
        return builder
            .AddPostClientEndpoint();
    }
}
