using CF.GameEngine.Web.Api.Features.ElementTypes.Get;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.GameEngine.Web.Api.Endpoints.ElementTypes.Get;

public static class Endpoints
{
    public static async Task<IResult> GetPagedElementsAsync(
        ElementTypeQuery query, IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);
        return result.ToApiResult();
    }

    public static async Task<IResult> GetElementTypeAsync([FromRoute] Guid id,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ElementTypeFindQuery(id), cancellationToken);
        return result.ToApiResult();
    }

    public static IEndpointRouteBuilder AddGetElementTypeEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/api/element-types", GetPagedElementsAsync)
            .WithName("GetElementTypes")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        builder.MapGet("/api/element-types/{id}", GetElementTypeAsync)
            .WithName("GetElementType")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return builder;
    }
}
