using CF.Identity.Api.Features.AccessRoles.Post;
using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Http.Authentication.Extensions;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.Identity.Api.Endpoints.AccessRoles.Post;

public static class Endpoint
{
    public static async Task<IResult> SaveRoleAsync([FromForm] PostRoleRequest request,
        IMediator mediator, IHttpContextAccessor httpContextAccessor,
        CancellationToken cancellationToken)
    {
        var data = request.ConvertToEditable();
        var result = await mediator.Send(new PostAccessRoleCommand(data), cancellationToken);
        return result.NegotiateResult(httpContextAccessor, Endpoints.BaseUrl);
    }

    public static IEndpointRouteBuilder AddPostRoleEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost(Endpoints.BaseUrl, SaveRoleAsync)
            .DisableAntiforgery()
            .RequireAuthorization(Authorise.Using<Roles>(RoleCategory.Write, SystemRoles.GlobalWrite));
        return builder;
    }
}
