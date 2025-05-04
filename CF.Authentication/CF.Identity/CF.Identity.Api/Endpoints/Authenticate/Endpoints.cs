using CF.Identity.Api.Features.Authenticate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.Identity.Api.Endpoints.Authenticate;

public static class Endpoints
{
    public static async Task<IResult> AuthenticateTokenAsync(
        [FromBody]AuthenticateCommand request, IMediator mediator,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);

        if (response.IsSuccessful)
        {
            return Results.Ok(new AuthenticationResponse(response));
        }

        return Results.Unauthorized();
    }

    public static async Task<IResult> RefreshExistingTokenAsync(
        [FromBody] RefreshExistingTokenCommand request, IMediator mediator,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        if (response.IsSuccessful)
        {
            return Results.Ok(new AuthenticationResponse(response));
        }

        return Results.Unauthorized();
    }

    public static IEndpointRouteBuilder AddAuthenticationEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/auth/authenticate", AuthenticateTokenAsync)
            .WithName("Authenticate")
            .Produces<AuthenticationResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        builder.MapPost("/auth/authenticate/refresh", RefreshExistingTokenAsync)
            .WithName("RefreshExistingToken")
            .Produces<AuthenticationResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        return builder;
    }
}
