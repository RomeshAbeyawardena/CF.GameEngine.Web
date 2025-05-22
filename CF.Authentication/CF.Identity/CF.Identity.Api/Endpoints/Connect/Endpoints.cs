namespace CF.Identity.Api.Endpoints.Connect;

public static class Endpoints
{
    public static IEndpointRouteBuilder AddConnectEndpoints(this IEndpointRouteBuilder builder)
    {
        return builder
            .AddIntrospectTokenEndpoint()
            .AddTokenRequestEndpoint()
            .AddUserInfoEndpoint();
    }
}
