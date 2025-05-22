using CF.Identity.Api.Extensions;
using CF.Identity.Api.Features.TokenExchange;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.Identity.Api.Endpoints.Connect;

public static class TokenEndpoint
{
    private static async Task<IResult> RequestTokenAsync([FromForm]TokenRequest tokenRequest, IHttpContextAccessor contextAccessor,
        IMediator mediator, CancellationToken cancellationToken)
    {
        //Scopes the current user has access to:
        var scopes = contextAccessor.HttpContext!.User.GetScopes();

        var response = await mediator.Send(new TokenRequestQuery(tokenRequest, scopes), cancellationToken);
        var result = response.GetResultOrDefault();

        if(result is not null)
        {
            if(response.TryGetValue("redirectUri", out var redirectUri) && redirectUri is not null)
            {
                return Results.Redirect(redirectUri.ToString()!);
            }

            return Results.Ok(result);
        }

        return Results.Unauthorized();
    }

    public static IEndpointRouteBuilder AddTokenRequestEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/connect/token", RequestTokenAsync)
            .Accepts<TokenRequest>("application/x-www-form-urlencoded")
            .DisableAntiforgery()
            .AllowAnonymous()
            .Produces<TokenResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            //.RequireRateLimiting("authentication-rate-limits");
        return builder;
    }
}
