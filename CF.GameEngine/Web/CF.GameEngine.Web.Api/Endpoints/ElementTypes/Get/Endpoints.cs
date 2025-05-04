using CF.GameEngine.Web.Api.Features.ElementTypes;
using CF.GameEngine.Web.Api.Features.ElementTypes.Get;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.GameEngine.Web.Api.Endpoints.ElementTypes.Get;

public static class Endpoints
{
    public static async Task<IResult> GetPagedElementTypesAsync(
        string? externalReference, string? key, string? nameContains, int? pageSize, int? pageIndex,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ElementTypeQuery(externalReference, key, nameContains, pageSize, pageIndex), cancellationToken);
        return result.ToApiResult(Route.BaseUrl);
    }

    public static async Task<IResult> GetElementTypeAsync([FromRoute] Guid id,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ElementTypeFindQuery(id), cancellationToken);
        return result.ToApiResult(Route.BaseUrl);
    }

    public static IEndpointRouteBuilder AddGetElementTypeEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(Route.BaseUrl, GetPagedElementTypesAsync)
            .WithName("GetElementTypes")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        builder.MapGet("{id:guid}".PrependUrl(Route.BaseUrl), GetElementTypeAsync)
            .WithName("GetElementType")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return builder;
    }
}
