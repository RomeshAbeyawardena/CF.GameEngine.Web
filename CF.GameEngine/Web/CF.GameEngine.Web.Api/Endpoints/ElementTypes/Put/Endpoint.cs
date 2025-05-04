using CF.GameEngine.Web.Api.Features.ElementTypes;
using CF.GameEngine.Web.Api.Features.ElementTypes.Put;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.GameEngine.Web.Api.Endpoints.ElementTypes.Put;

public static class Endpoint
{
    public static async Task<IResult> UpdateElementTypeAsync(Guid id,
      [FromForm] ElementTypeDto elementType,
      IMediator mediator,
      CancellationToken cancellationToken)
    {
        elementType.Id = id;
        var result = await mediator.Send(new PutElementTypeCommand(elementType), cancellationToken);
        return result.ToApiResult(Route.BaseUrl);
    }

    public static IEndpointRouteBuilder AddPutElementTypeEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPut("{id:guid}".PrependUrl(Route.BaseUrl), UpdateElementTypeAsync)
            .WithName(nameof(UpdateElementTypeAsync))
            .WithTags(Route.Tag)
            .DisableAntiforgery()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        return builder;
    }
}
