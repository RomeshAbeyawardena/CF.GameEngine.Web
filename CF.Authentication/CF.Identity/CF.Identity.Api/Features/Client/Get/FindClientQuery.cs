using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Client.Get;

public record FindClientQuery(Guid ClientId) : IUnitRequest<ClientDetailResponse>;

public class FindClientQueryHandler(IClientRepository clientRepository) : IUnitRequestHandler<FindClientQuery, ClientDetailResponse>
{
    public async Task<IUnitResult<ClientDetailResponse>> Handle(FindClientQuery request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetByClientId(request.ClientId, cancellationToken);
        return client.Convert(x => x.Map<ClientDetailResponse>());
    }
}
