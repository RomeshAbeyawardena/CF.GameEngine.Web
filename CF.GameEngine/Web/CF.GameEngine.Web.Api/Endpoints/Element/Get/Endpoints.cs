using CF.GameEngine.Web.Api.Features.Element.Get;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Http.Extensions;
using MediatR;

namespace CF.GameEngine.Web.Api.Endpoints.Element.Get;

public static class Endpoints
{
    public static async Task<IResult> GetPagedElementsAsync(
        Guid? parentId, string? externalReference,
        string? key, string? nameContains, int? pageSize, int? pageIndex,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ElementQuery(parentId, externalReference, key, nameContains, pageSize, pageIndex), cancellationToken);

        return result.ToApiResult(Route.BaseUrl);
    }

    public static async Task<IResult> FindElementAsync(Guid id,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ElementFindQuery(id), cancellationToken);
        return result.ToApiResult(Route.BaseUrl);
    }

    public static IEndpointRouteBuilder AddGetElementEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(Route.BaseUrl, GetPagedElementsAsync)
            .WithName("GetPagedElements")
            .Produces(200)
            .Produces(400)
            .Produces(500)
            .WithTags("Elements");

        builder.MapGet("{id:guid}".PrependUrl(Route.BaseUrl), FindElementAsync)
            .WithName("FindElement")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .WithTags("Elements");
        return builder;
    }
}
