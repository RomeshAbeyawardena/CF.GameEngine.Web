using CF.Identity.Api.Features.User;
using CF.Identity.Api.Features.User.Post;
using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CF.Identity.Api.Endpoints.Users.Post;

public static class Endpoint
{
    public static async Task<IResult> SaveUserAsync([FromForm] PostUserRequest request,
        IHttpContextAccessor httpContextAccessor,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var data = request.MapToEditable();
        
        var result = await mediator.Send(new PostUserCommand(data), cancellationToken);
        return result.NegotiateResult(httpContextAccessor, Endpoints.Url);
    }

    public static IEndpointRouteBuilder AddPostEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost(Endpoints.Url, SaveUserAsync)
            .DisableAntiforgery()
            .RequireAuthorization(new AuthorizeAttribute(RoleDescriptor.ConcatenateRoles(',',
                SystemRoles.GlobalWrite,
                UserRoles.UserWrite)));
        return builder;
    }
}
