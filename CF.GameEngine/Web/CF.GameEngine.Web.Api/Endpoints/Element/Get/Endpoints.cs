using CF.GameEngine.Web.Api.Features.Element.Get;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Http.Extensions;
using MediatR;

namespace CF.GameEngine.Web.Api.Endpoints.Element.Get;

public static class Endpoints
{
    public static async Task<IResult> GetPagedElementsAsync(
        [AsParameters]ElementQuery query,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);

        return result.ToHypermediaResult(Route.BaseUrl);
    }

    public static async Task<IResult> FindElementAsync(Guid id,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ElementFindQuery(id), cancellationToken);
        return result.ToHypermediaResult(Route.BaseUrl);
    }

    public static IEndpointRouteBuilder AddGetElementEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(Route.BaseUrl, GetPagedElementsAsync)
            .WithName(nameof(GetPagedElementsAsync))
            .Produces(200)
            .Produces(400)
            .Produces(500)
            .WithTags(Route.Tag);

        builder.MapGet("{id:guid}".PrependUrl(Route.BaseUrl), FindElementAsync)
            .WithName(nameof(FindElementAsync))
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .WithTags(Route.Tag);
        return builder;
    }
}
