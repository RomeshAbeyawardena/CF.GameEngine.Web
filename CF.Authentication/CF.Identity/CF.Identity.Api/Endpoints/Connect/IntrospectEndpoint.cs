using CF.Identity.Api.Features.Introspect;
using IDFCR.Http.Authentication;
using IDFCR.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.Identity.Api.Endpoints.Connect;

public static class IntrospectEndpoint
{
    public static async Task<IResult> IntrospectTokenAsync([FromForm] string token,
        IHttpContextAccessor contextAccessor,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var client = contextAccessor.HttpContext!.User.GetClient();
        var result = (await mediator.Send(new IntrospectQuery(token, client.Id.GetValueOrDefault()), cancellationToken))
            .GetResultOrDefault();

        if (result is null)
        {
            return Results.Ok(new IntrospectBaseResponse(false));
        }

        return Results.Ok(result);
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
