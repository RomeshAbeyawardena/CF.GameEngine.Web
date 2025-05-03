using CF.GameEngine.Web.Api.Endpoints.ElementTypes;

namespace CF.GameEngine.Web.Api.Endpoints;

public static class Endpoints
{
    public static IEndpointRouteBuilder AddApiEndpoints(this IEndpointRouteBuilder builder)
    {
        return builder.AddElementTypeEndpoints();
    }
}
