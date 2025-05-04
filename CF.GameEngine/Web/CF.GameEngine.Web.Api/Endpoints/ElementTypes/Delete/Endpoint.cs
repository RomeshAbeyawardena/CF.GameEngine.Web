using CF.GameEngine.Web.Api.Features.ElementTypes.Delete;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.GameEngine.Web.Api.Endpoints.ElementTypes.Delete;

public static class Endpoint
{
    public static async Task<IResult> DeleteElementTypeAsync([FromRoute] Guid id,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ElementTypeDeleteCommand(id), cancellationToken);
        return result.ToApiResult();
    }

    public static IEndpointRouteBuilder AddDeleteElementTypeEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapDelete("{id:guid}".PrependUrl(Route.BaseUrl), DeleteElementTypeAsync)
            .WithName("DeleteElementType")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags("Element Types");
        return builder;

    }
}