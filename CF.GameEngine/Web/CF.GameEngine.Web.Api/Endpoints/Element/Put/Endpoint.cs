using CF.GameEngine.Web.Api.Features.Element;
using CF.GameEngine.Web.Api.Features.Element.Put;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.GameEngine.Web.Api.Endpoints.Element.Put;

public static class Endpoint
{
    public static async Task<IResult> PutElementAsync([FromForm] ElementDto element,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new PutElementCommand(element), cancellationToken);
        return result.ToApiResult(Route.BaseUrl);
    }

    public static IEndpointRouteBuilder AddPutElementEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPut(Route.BaseUrl, PutElementAsync)
            .WithName(nameof(PutElementAsync))
            .WithTags(Route.Tag)
            .DisableAntiforgery()
            .Produces<Guid>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return builder;
    }
}
