namespace CF.Identity.Api.Endpoints.Connect;

public static class Endpoints
{
    public static IEndpointRouteBuilder AddConnectEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.AddTokenRequestEndpoint();
        builder.AddIntrospectTokenEndpoint();
        return builder;
    }
}
