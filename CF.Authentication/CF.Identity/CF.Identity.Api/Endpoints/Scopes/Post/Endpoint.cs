using CF.Identity.Api.Features.Scopes.Post;
using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CF.Identity.Api.Endpoints.Scopes.Post;

public static class Endpoint
{
    public static async Task<IResult> SaveScopeAsync([FromForm] PostScopeRequest request, IHttpContextAccessor contextAccessor,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new PostScopeCommand(request.ToEditable()), cancellationToken);
        return result.NegotiateResult(contextAccessor, Endpoints.BaseUrl);
    }

    public static IEndpointRouteBuilder AddPostScopeEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost(Endpoints.BaseUrl, SaveScopeAsync)
            .RequireAuthorization(new AuthorizeAttribute(RoleRegistrar.FlattenedRoles<ScopeRoles>(RoleCategory.Write, SystemRoles.GlobalWrite)));

        return builder;
    }
}
