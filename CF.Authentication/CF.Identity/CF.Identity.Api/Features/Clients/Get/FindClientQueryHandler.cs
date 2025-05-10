using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Clients.Get;

public class FindClientQueryHandler(IClientRepository clientRepository) : IUnitRequestCollectionHandler<FindClientQuery, ClientDetailResponse>
{
    public async Task<IUnitResultCollection<ClientDetailResponse>> Handle(FindClientQuery request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetClients(request, cancellationToken);
        return client.Convert(x => x.Map<ClientDetailResponse>());
    }
}
