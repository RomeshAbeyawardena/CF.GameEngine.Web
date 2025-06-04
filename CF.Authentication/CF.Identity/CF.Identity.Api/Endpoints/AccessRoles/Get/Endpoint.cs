using CF.Identity.Api.Features.AccessRoles.List;
using CF.Identity.Infrastructure.Features;
using IDFCR.Http.Authentication.Extensions;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Roles = CF.Identity.Infrastructure.Features.AccessRoles;

namespace CF.Identity.Api.Endpoints.AccessRoles.Get;

public static class Endpoint
{
    public static async Task<IResult> GetRoles(
        [AsParameters] GetRolesRequest request, IHttpContextAccessor httpContextAccessor,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request.Map<ListAccessRolesQuery>(), cancellationToken);

        return result.ToApiCollectionResult(Endpoints.BaseUrl);//NegotiateResult(httpContextAccessor, Endpoints.BaseUrl);
    }

    public static IEndpointRouteBuilder AddGetRolesEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(Endpoints.BaseUrl, GetRoles)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .WithDescription("Retrieves a list of access roles.")
            .WithName("GetAccessRoles")
            .WithTags("Access Roles")
            .RequireAuthorization(Authorise.Using<Roles.Roles>(RoleCategory.Read, SystemRoles.GlobalRead));
        return builder;
    }
}
