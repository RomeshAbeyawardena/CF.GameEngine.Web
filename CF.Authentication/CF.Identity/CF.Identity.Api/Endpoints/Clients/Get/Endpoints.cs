using CF.Identity.Api.Features.Client;
using CF.Identity.Api.Features.Client.Get;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Http.Extensions;
using MediatR;

namespace CF.Identity.Api.Endpoints.Clients.Get;

public static class Endpoints
{
    public static async Task<IResult> GetClientAsync(Guid id,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindClientQuery(id), cancellationToken);
        return result.ToApiResult(Route.BaseUrl);
    }

    //public static async Task<IResult> GetPagedClientsAsync()

    public static void AddClientEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("{id:guid}".PrependUrl(Route.BaseUrl), GetClientAsync)
            .WithName(nameof(GetClientAsync))
            .Produces<ClientDetailResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(Route.Tag);
    }
}
