using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Results;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal class ClientRepository(TimeProvider timeProvider, CFIdentityDbContext context) : RepositoryBase<IClient, Client, ClientDto>(timeProvider, context), IClientRepository
{
    public async Task<IUnitResult<ClientDto>> GetByClientId(Guid clientId, CancellationToken cancellationToken)
    {
        var client = await FindAsync(cancellationToken, [clientId]);
        if (client is null)
        {
            return UnitResult.NotFound<ClientDto>(clientId);
        }

        return new UnitResult<ClientDto>(client, UnitAction.Get);
    }
}
