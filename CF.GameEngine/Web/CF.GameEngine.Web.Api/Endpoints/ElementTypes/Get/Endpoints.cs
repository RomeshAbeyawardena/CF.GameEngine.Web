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
    }

    public static async Task<IResult> GetElementTypeAsync([FromRoute] Guid id,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ElementTypeFindQuery(id), cancellationToken);
        return result.ToApiResult();
    }
}
