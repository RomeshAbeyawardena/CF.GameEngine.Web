using CF.GameEngine.Web.Api.Features.Element.Delete;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.GameEngine.Web.Api.Endpoints.Element.Delete;

public static class Endpoint
{
    public static async Task<IResult> DeleteElementAsync([FromRoute]Guid id,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteElementCommand(id), cancellationToken);
        return result.ToApiResult();
    }

    public static IEndpointRouteBuilder AddDeleteElement(this IEndpointRouteBuilder builder)
    {
        builder.MapDelete("/elements/{id:guid}", DeleteElementAsync)
            .WithName("DeleteElement")
            .Produces(204)
            .Produces(404)
            .WithTags("Elements");
        return builder;
    }
}
