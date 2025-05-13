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

        var authorisation = context.Request.Headers.Authorization.FirstOrDefault();
        var auth = context.Request.Headers["x-auth"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(auth)
            || string.IsNullOrWhiteSpace(authorisation)
            || !authorisation.StartsWith("Bearer", StringComparison.InvariantCultureIgnoreCase))
        {
            return Results.Unauthorized();
        }

        var accessToken = authorisation["Bearer ".Length..].Trim();
        var raw = Encoding.UTF8.GetString(Convert.FromBase64String(auth));

        var parts = raw.Split(':', 2);
        if (parts.Length != 2)
        {
            return Results.Unauthorized();
        }

        var clientId = parts[0];
        var clientSecret = parts[1];

        if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
        {
            return Results.Unauthorized();
        }

        var clientResult = (await mediator.Send(new FindClientQuery(clientId), cancellationToken)).GetOneOrDefault();

        if(clientResult is null 
            || clientCredentialHasher.Verify(clientSecret, clientResult))
        {
            return Results.Unauthorized();
        }

        var result = await mediator.Send(new UserInfoRequest(accessToken, clientId), cancellationToken);

        return Results.Ok(result.GetResultOrDefault());
    }

    public static IEndpointRouteBuilder AddUserInfoEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/connect/userinfo", GetUserInfoAsync)
            .Produces<UserInfoResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireRateLimiting("authentication-rate-limits");
        return builder;
    }
}
