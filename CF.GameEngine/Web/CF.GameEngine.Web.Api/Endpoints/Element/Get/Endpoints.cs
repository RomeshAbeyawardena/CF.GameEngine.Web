using CF.GameEngine.Web.Api.Features.Element.Get;
using IDFCR.Shared.Http.Extensions;
using MediatR;

namespace CF.GameEngine.Web.Api.Endpoints.Element.Get;

public static class Endpoints
{
    public static async Task<IResult> GetPagedElementsAsync(
        Guid? parentId, string? externalReference,
        string? key, string? nameContains, int? pageSize, int? pageIndex,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ElementQuery(parentId, externalReference, key, nameContains, pageSize, pageIndex), cancellationToken);

        return result.ToApiResult(string.Empty);
    }
}
