using CF.Identity.Api.Features.AccessRoles.List;
using IDFCR.Shared.Http.Extensions;
using MediatR;

namespace CF.Identity.Api.Endpoints.AccessRoles.Get;

public static class Endpoint
{
    public static async Task<IResult> GetRoles(
        [AsParameters] GetRolesRequest request, IHttpContextAccessor httpContextAccessor,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request.Map<ListAccessRolesQuery>(), cancellationToken);

        return result.NegotiateResult(httpContextAccessor, Endpoints.BaseUrl);
    }
}
