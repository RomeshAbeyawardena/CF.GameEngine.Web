using CF.Identity.Api.Features.Introspect;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.Identity.Api.Endpoints.Connect;

public static class IntrospectEndpoint
{
    public static async Task<IResult> IntrospectTokenAsync([FromForm]string token, 
        IHttpContextAccessor contextAccessor,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var authenticationResult = await contextAccessor.TryAuthenticateAsync(cancellationToken);
        if (authenticationResult.success && authenticationResult.client is not null)
        {
            var result = await mediator.Send(new IntrospectQuery(token, authenticationResult.client), cancellationToken);

            if (!result.IsSuccess)
            {
                return Results.Ok(new IntrospectBaseResponse(false));
            }

            return Results.Ok(result.Result);
        }

        return Results.Unauthorized();
    }
}
