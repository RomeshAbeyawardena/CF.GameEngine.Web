using CF.Identity.Api.Features.Client.Get;
using MediatR;

namespace CF.Identity.Api.Endpoints.Clients.Get;

public static class Endpoints
{
    public static async Task<IResult> GetClientAsync(Guid id,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindClientQuery(id), cancellationToken);
    }
}
