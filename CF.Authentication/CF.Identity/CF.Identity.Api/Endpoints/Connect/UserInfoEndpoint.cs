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

        var authenticatedClient = context.GetAuthenticatedClient();

        var authorisation = context.Request.Headers.Authorization.FirstOrDefault();

        if (authenticatedClient is null
            || string.IsNullOrWhiteSpace(authorisation)
            || !authorisation.StartsWith("Bearer", StringComparison.InvariantCultureIgnoreCase))
        {
            return Results.Unauthorized();
        }

        var accessToken = authorisation["Bearer ".Length..].Trim();

        var result = await mediator.Send(new UserInfoRequest(accessToken, authenticatedClient.ClientId), cancellationToken);

        if (!result.IsSuccess)
        {
            return Results.Unauthorized();
        }

        return Results.Ok(result.GetResultOrDefault());
    }

    public static IEndpointRouteBuilder AddUserInfoEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/connect/userinfo", GetUserInfoAsync)
            .Produces<UserInfoResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            //.RequireRateLimiting("authentication-rate-limits");
        return builder;
    }
}
