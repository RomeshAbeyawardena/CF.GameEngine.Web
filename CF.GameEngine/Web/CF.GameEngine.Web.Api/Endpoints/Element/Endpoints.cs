using CF.GameEngine.Web.Api.Endpoints.Element.Delete;
using CF.GameEngine.Web.Api.Endpoints.Element.Get;
using CF.GameEngine.Web.Api.Endpoints.Element.Post;
using CF.GameEngine.Web.Api.Endpoints.Element.Put;

namespace CF.GameEngine.Web.Api.Endpoints.Element;

public static class Endpoints
{
    public static IEndpointRouteBuilder AddElementEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        return endpointRouteBuilder
            .AddDeleteElementEndpoint()
            .AddGetElementEndpoints()
            .AddPostElementEndpoint()
            .AddPutElementEndpoint();
    }
}
