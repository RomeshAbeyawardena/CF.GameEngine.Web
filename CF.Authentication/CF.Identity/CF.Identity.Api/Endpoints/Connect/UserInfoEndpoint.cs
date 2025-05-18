using CF.Identity.Api.Extensions;
using CF.Identity.Api.Features.Clients.Get;
using CF.Identity.Api.Features.User.Info;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Extensions;
using MediatR;
using System.Text;

namespace CF.Identity.Api.Endpoints.Connect;

public static class UserInfoEndpoint
{
    public static async Task<IResult> GetUserInfoAsync(IHttpContextAccessor httpContextAccessor, 
        IMediator mediator, IClientCredentialHasher clientCredentialHasher, CancellationToken cancellationToken)
    {
        var context = httpContextAccessor.HttpContext
            ?? throw new NullReferenceException("HttpContext not available in this context");

        var id = context.User.Identities;

        //var authenticatedClient = context.GetAuthenticatedClient();
        //var accessToken = context.GetAccessToken();
        //if (authenticatedClient is null || accessToken is null)
        //{
        //    return Results.Unauthorized();
        //}

        ////var result = await mediator.Send(new UserInfoRequest(accessToken, authenticatedClient.ClientDetails), cancellationToken);

        //if (!result.IsSuccess)
        //{
        //    return Results.Unauthorized();
        //}

        //return Results.Ok(result.GetResultOrDefault());
        return Results.Ok();
    }

    public static IEndpointRouteBuilder AddUserInfoEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/connect/userinfo", GetUserInfoAsync)
            .Produces<UserInfoResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();
            //.RequireRateLimiting("authentication-rate-limits");
        return builder;
    }
}
