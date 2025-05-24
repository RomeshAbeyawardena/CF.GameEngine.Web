using CF.Identity.Api.Features.Scopes.Post;
using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CF.Identity.Api.Endpoints.Roles.Post;

public static class Endpoint
{
    public static async Task<IResult> SaveScopeAsync([FromForm] PostRoleRequest request,
        IMediator mediator, IHttpContextAccessor httpContextAccessor,
        CancellationToken cancellationToken)
    {
        var data = request.ConvertToEditable();
        var result = await mediator.Send(new PostScopeCommand(data), cancellationToken);
        return result.NegotiateResult(httpContextAccessor, Endpoints.BaseUrl);
    }

    public static IEndpointRouteBuilder AddPostEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost(Endpoints.BaseUrl, SaveScopeAsync)
            .DisableAntiforgery()
            .RequireAuthorization(new AuthorizeAttribute(RoleDescriptor.ConcatenateRoles(',',
                SystemRoles.GlobalWrite,
                ScopeRoles.ScopeWrite)));
        return builder;
    }
}
