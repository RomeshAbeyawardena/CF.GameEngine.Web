using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Client.Get;

public class FindClientByIdQueryHandler(IClientRepository clientRepository) : IUnitRequestHandler<FindClientByIdQuery, ClientDetailResponse>
{
    public async Task<IUnitResult<ClientDetailResponse>> Handle(FindClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetByClientId(request.ClientId, cancellationToken);
        return client.Convert(x => x.Map<ClientDetailResponse>());
    }
}
