using CF.Identity.Api.Extensions;
using CF.Identity.Api.Features.User.Info;
using CF.Identity.Infrastructure.Features.Clients;
using Microsoft.AspNetCore.Authorization;

namespace CF.Identity.Api.Endpoints.Connect;

public static class UserInfoEndpoint
{
    public static async Task<IResult> GetUserInfoAsync(IHttpContextAccessor httpContextAccessor, IClientCredentialHasher clientCredentialHasher, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        var context = httpContextAccessor.HttpContext
            ?? throw new NullReferenceException("HttpContext not available in this context");

        var userId = context.User.GetUserId() 
            ?? throw new NullReferenceException();
        var name = context.User.GetUserDisplayName() 
            ?? throw new NullReferenceException();
        var username = context.User.GetUserName() 
            ?? throw new NullReferenceException();
        var email = context.User.GetUserEmail() 
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
