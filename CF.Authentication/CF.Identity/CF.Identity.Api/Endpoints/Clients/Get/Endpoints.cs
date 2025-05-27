using CF.Identity.Api.Features.Clients;
using CF.Identity.Api.Features.Clients.Get;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.CompilerServices;

namespace CF.Identity.Api.Endpoints.Clients.Get;

public static class Endpoints
{
    public static async Task<IResult> GetClientAsync(Guid id,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindClientByIdQuery(id), cancellationToken);
        return result.ToApiResult(Route.BaseUrl);
    }

    //public static async Task<IResult> GetPagedClientsAsync()

    public static IEndpointRouteBuilder AddGetClientEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("{id:guid}".PrependUrl(Route.BaseUrl), GetClientAsync)
            .WithName(nameof(GetClientAsync))
            .Produces<ClientDetailResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(new AuthorizeAttribute(RoleRegistrar.FlattenedRoles<ClientRoles>()))
            .WithTags(Route.Tag);
        return app;
    }
}
