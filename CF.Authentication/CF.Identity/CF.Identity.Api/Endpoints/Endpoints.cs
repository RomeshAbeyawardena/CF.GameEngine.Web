using CF.Identity.Api.Endpoints.Clients;
using CF.Identity.Api.Endpoints.Connect;
using CF.Identity.Api.Endpoints.Scopes;
using CF.Identity.Api.Endpoints.Users;

namespace CF.Identity.Api.Endpoints;

public static class Endpoints
{
    public static IEndpointRouteBuilder AddEndpoints(this IEndpointRouteBuilder builder)
    {
        return builder
            .AddClientEndpoints()
            .AddConnectEndpoints()
            .AddScopeEndpoints()
            .AddUserEndpoints();
    }
}
