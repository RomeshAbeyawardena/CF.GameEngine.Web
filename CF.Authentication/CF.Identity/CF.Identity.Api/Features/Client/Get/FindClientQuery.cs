using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Client.Get;

public record FindClientQuery(string? Key, bool NoTracking = true) : IUnitRequestCollection<ClientDetailResponse>, IClientFilter;

public class FindClientQueryHandler(IClientRepository clientRepository) : IUnitRequestCollectionHandler<FindClientQuery, ClientDetailResponse>
{
    public async Task<IUnitResultCollection<ClientDetailResponse>> Handle(FindClientQuery request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetClients(request, cancellationToken);
        return client.Convert(x => x.Map<ClientDetailResponse>());
    }
}


public record FindClientByIdQuery(Guid ClientId) : IUnitRequest<ClientDetailResponse>;

public class FindClientByIdQueryHandler(IClientRepository clientRepository) : IUnitRequestHandler<FindClientByIdQuery, ClientDetailResponse>
{
    public async Task<IUnitResult<ClientDetailResponse>> Handle(FindClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetByClientId(request.ClientId, cancellationToken);
        return client.Convert(x => x.Map<ClientDetailResponse>());
    }
}
