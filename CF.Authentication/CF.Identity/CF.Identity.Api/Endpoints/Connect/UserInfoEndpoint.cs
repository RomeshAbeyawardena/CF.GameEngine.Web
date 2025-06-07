using CF.Identity.Api.Features.User.Info;
using IDFCR.Http.Authentication;
using IDFCR.Http.Authentication.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace CF.Identity.Api.Endpoints.Connect;

public static class UserInfoEndpoint
{
    public static async Task<IResult> GetUserInfoAsync(IAuthenticatedUserContext authenticatedUserContext, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var userId = authenticatedUserContext.User?.GetUserId()
            ?? throw new NullReferenceException();
        var name = authenticatedUserContext.User?.GetUserDisplayName()
            ?? throw new NullReferenceException();
        var username = authenticatedUserContext.User?.GetUserName()
            ?? throw new NullReferenceException();
        var email = authenticatedUserContext.User?.GetUserEmail()
            ?? throw new NullReferenceException();

        return Results.Ok(
            new UserInfoResponse(userId, name, username, email)
            );
    }

    public static IEndpointRouteBuilder AddUserInfoEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/connect/userinfo", GetUserInfoAsync)
            .Produces<UserInfoResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization(new AuthorizeAttribute());
        //.RequireRateLimiting("authentication-rate-limits");
        return builder;
    }
}
