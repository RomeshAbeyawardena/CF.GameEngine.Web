using CF.Identity.Api.Endpoints.Users.Post;

namespace CF.Identity.Api.Endpoints.Users;

public static class Endpoints
{
    public const string Url = "/api/user";
    public static IEndpointRouteBuilder AddUserEndpoints(this IEndpointRouteBuilder builder)
    {
        return builder.AddPostEndpoint();
    }
}
