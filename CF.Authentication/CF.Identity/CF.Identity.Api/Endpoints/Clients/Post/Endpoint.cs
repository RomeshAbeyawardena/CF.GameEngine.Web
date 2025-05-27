using CF.Identity.Api.Features.Clients;
using CF.Identity.Api.Features.Clients.Post;
using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Http.Authentication.Extensions;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.Identity.Api.Endpoints.Clients.Post;

public static class Endpoint
{
    public static async Task<IResult> SaveClientAsync([FromForm] PostClientRequest request, 
        IMediator mediator, IHttpContextAccessor httpContextAccessor,
        CancellationToken cancellationToken)
    {
        var data = request.Map<EditableClientDto>();
        
        var result = await mediator.Send(new PostClientCommand(data), cancellationToken);
        return result.NegotiateResult(httpContextAccessor, Endpoints.Url);
    }

    public static IEndpointRouteBuilder AddPostClientEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost(Route.BaseUrl, SaveClientAsync)
            .DisableAntiforgery()
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .WithDescription("Creates a new client.")
            .RequireAuthorization(Authorise.Using<ClientRoles>(RoleCategory.Write, SystemRoles.GlobalWrite));
        return builder;
    }
}
