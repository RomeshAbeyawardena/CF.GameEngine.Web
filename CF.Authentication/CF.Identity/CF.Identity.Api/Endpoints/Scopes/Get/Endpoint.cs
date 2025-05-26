using IDFCR.Http.Authentication.Abstractions;
using IDFCR.Shared.Http.Extensions;
using MediatR;

namespace CF.Identity.Api.Endpoints.Scopes.Get;

public static class Endpoint
{
    public static async Task<IResult> GetScopePagedAsync([AsParameters] GetScopesRequest request, IAuthenticatedUserContext userContext,
       IMediator mediator, IHttpContextAccessor contextAccessor, CancellationToken cancellationToken)
    {
        if (!userContext.Client?.IsSystem ?? false)
        {
            request.ClientId = userContext.Client?.Id;
        }

        var query = request.ToQuery();
        var result = await mediator.Send(query, cancellationToken);
        return result.NegotiateResult(contextAccessor, Endpoints.BaseUrl);
    }
}
