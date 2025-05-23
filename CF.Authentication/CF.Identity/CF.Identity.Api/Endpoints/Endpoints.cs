using CF.Identity.Api.Endpoints.Clients.Get;
using CF.Identity.Api.Endpoints.Connect;
using CF.Identity.Api.Endpoints.Users;

namespace CF.Identity.Api.Endpoints;

public static class Endpoints
{
    public static IEndpointRouteBuilder AddEndpoints(this IEndpointRouteBuilder builder)
    {
        return builder
            .AddClientEndpoints()
            .AddConnectEndpoints()
            .AddUserEndpoints();
    }
}
