using CF.GameEngine.Web.Api.Endpoints.ElementTypes.Delete;
using CF.GameEngine.Web.Api.Endpoints.ElementTypes.Get;
using CF.GameEngine.Web.Api.Endpoints.ElementTypes.Post;
using CF.GameEngine.Web.Api.Endpoints.ElementTypes.Put;

namespace CF.GameEngine.Web.Api.Endpoints.ElementTypes;

public static class Endpoints
{
    public static IEndpointRouteBuilder AddElementTypeEndpoints(this IEndpointRouteBuilder builder)
    {
        return builder.AddDeleteElementTypeEndpoint()
            .AddGetElementTypeEndpoints()
            .AddPostElementTypeEndpoint()
            .AddPutElementTypeEndpoint();
    }
}
