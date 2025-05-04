using CF.GameEngine.Web.Api.Features.Element;
using CF.GameEngine.Web.Api.Features.Element.Post;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.GameEngine.Web.Api.Endpoints.Element.Post;

public static class Endpoint
{
    public static async Task<IResult> PostElementAsync([FromForm]ElementDto element,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new PostElementCommand(element), cancellationToken);
        return result.ToApiResult(Route.BaseUrl);
    }
}
