using CF.Identity.Api.Features.Clients;
using CF.Identity.Api.Features.Clients.Get;
using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Http.Authentication.Extensions;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CF.Identity.Api.Endpoints.Clients.Get;

public static class Endpoint
{
    public static async Task<IResult> GetClientAsync([FromRoute]Guid id, IHttpContextAccessor httpContextAccessor,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindClientByIdQuery(id), cancellationToken);
        return result.NegotiateResult(httpContextAccessor, Endpoints.BaseUrl);
    }

    //public static async Task<IResult> GetPagedClientsAsync()

    public static IEndpointRouteBuilder AddGetClientEndpoints(this IEndpointRouteBuilder app)
    {
        var url = "{id:guid}".PrependUrl(Endpoints.BaseUrl);
        app.MapGet(url, GetClientAsync)
            .WithName(Endpoints.GetClient)
            .Produces<ClientDetailResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(Authorise.Using<ClientRoles>(RoleCategory.Read, SystemRoles.GlobalRead))
            .WithTags(Endpoints.Tag);
        return app;
    }
}
