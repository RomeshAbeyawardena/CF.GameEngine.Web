using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.GameEngine.Web.Api.Endpoints.Element.Delete;

public static class Endpoint
{
    public static Task<IResult> DeleteElementAsync([FromRoute]Guid id,
        IMediator mediator, CancellationToken cancellationToken)
    {
        
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
