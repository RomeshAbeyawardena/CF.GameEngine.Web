using CF.GameEngine.Web.Api.Features.ElementTypes;
using CF.GameEngine.Web.Api.Features.ElementTypes.Post;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.GameEngine.Web.Api.Endpoints.ElementTypes.Post;

public static class Endpoint
{
    public static async Task<IResult> SaveElementTypeAsync(
        [FromForm] ElementTypeDto elementType,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new PostElementTypeCommand(elementType), cancellationToken);
        return result.ToApiResult();
    }

    public static IEndpointRouteBuilder AddPostElementTypeEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/api/element-types", SaveElementTypeAsync)
            .WithName("SaveElementType")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        return builder;
    }
}
