using IDFCR.Shared.Abstractions.Repositories;
using IDFCR.Shared.Abstractions.Results;

namespace CF.Identity.Infrastructure.Features.Clients;

public interface IClientRepository : IRepository<ClientDto>
{
    Task<IUnitResult<ClientDto>> GetByClientId(Guid clientId, CancellationToken cancellationToken);
}
