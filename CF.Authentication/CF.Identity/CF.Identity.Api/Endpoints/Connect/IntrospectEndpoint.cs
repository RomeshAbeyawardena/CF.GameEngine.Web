using CF.Identity.Api.Features.Introspect;
using IDFCR.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.Identity.Api.Endpoints.Connect;

public static class IntrospectEndpoint
{
    public static async Task<IResult> IntrospectTokenAsync([FromForm]string token, 
        IHttpContextAccessor contextAccessor,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var (success, client) = await contextAccessor.TryAuthenticateAsync(cancellationToken);
        if (success && client is not null)
        {
            var result = (await mediator.Send(new IntrospectQuery(token, client), cancellationToken))
                .GetResultOrDefault();

            if (result is null)
            {
                return Results.Ok(new IntrospectBaseResponse(false));
            }

            return Results.Ok(result);
        }

        return Results.Unauthorized();
    }

    public static IEndpointRouteBuilder AddIntrospectTokenEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/connect/introspect", IntrospectTokenAsync)
            .Accepts<IntrospectQuery>("application/x-www-form-urlencoded")
            .Produces<IntrospectResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireRateLimiting("authentication-rate-limits");
        return builder;
    }
}
