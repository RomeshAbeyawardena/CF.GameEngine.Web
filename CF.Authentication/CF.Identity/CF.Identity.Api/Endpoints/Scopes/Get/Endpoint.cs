using CF.Identity.Api.Features.Scopes.Get;
using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Http.Authentication.Abstractions;
using IDFCR.Http.Authentication.Extensions;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.Identity.Api.Endpoints.Scopes.Get;

public static class Endpoint
{
    public static async Task<IResult> GetScopeAsync([FromRoute]Guid id, IMediator mediator, IHttpContextAccessor contextAccessor, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindScopeByIdQuery(id), cancellationToken);
        return result.NegotiateResult(contextAccessor, Endpoints.BaseUrl);
    }

    public static async Task<IResult> GetScopePagedAsync(
        [AsParameters] GetScopesRequest request,
        IAuthenticatedUserContext userContext, IMediator mediator, IHttpContextAccessor contextAccessor, CancellationToken cancellationToken)
    {
        //Non-system clients should not be accessing other tenant data!
        if (!userContext.Client?.IsSystem ?? false)
        {
            request.ClientId = userContext.Client?.Id;
        }

        var query = request.ToQuery();
        var result = await mediator.Send(query, cancellationToken);
        return result.NegotiateResult(contextAccessor, Endpoints.BaseUrl);
    }

    public static IEndpointRouteBuilder AddGetScopesEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("{id:guid}".PrependUrl(Endpoints.BaseUrl), GetScopeAsync)
            .RequireAuthorization(Authorise.Using<ScopeRoles>(RoleCategory.Read, SystemRoles.GlobalRead))
            .WithName(Endpoints.GetScope);

        builder.MapGet(Endpoints.BaseUrl, GetScopePagedAsync)
            .RequireAuthorization(Authorise.Using<ScopeRoles>(RoleCategory.Read, SystemRoles.GlobalRead))
            .WithName(Endpoints.GetPagedScope)
            .WithSummary("Get paged scopes")
            .WithDescription("Retrieves a paginated list of scopes based on the provided filter criteria.");
        return builder;
    }
}
