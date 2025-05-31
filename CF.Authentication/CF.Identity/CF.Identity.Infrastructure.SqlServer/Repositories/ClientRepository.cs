using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.SqlServer.Filters;
using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.SPA;
using IDFCR.Shared.Abstractions.Results;
using Microsoft.EntityFrameworkCore;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal class ClientRepository(IClientProtection clientCredentialHasher,
    TimeProvider timeProvider, CFIdentityDbContext context) : RepositoryBase<IClient, DbClient, ClientDto>(timeProvider, context), IClientRepository
{
    protected override void OnAdd(DbClient db, ClientDto source)
    {
        clientCredentialHasher.Protect(db);
        base.OnAdd(db, source);
    }

    public async Task<IUnitResult<ClientDto>> GetByClientId(Guid clientId, CancellationToken cancellationToken)
    {
        var client = await FindAsync(cancellationToken, [clientId]);
        if (client is null)
        {
            return UnitResult.NotFound<ClientDto>(clientId);
        }

        return UnitResult.FromResult(client, UnitAction.Get);
    }

    public async Task<IUnitResultCollection<ClientDto>> GetClients(IClientFilter filter, CancellationToken cancellationToken)
    {
        var clientFilter = new ClientFilter(filter);

        var result = await Set<DbClient>(filter)
            .Where(clientFilter.ApplyFilter(Builder, filter))
            .ToListAsync(cancellationToken);

        return UnitResultCollection.FromResult(MapTo(result), UnitAction.Get);
    }
}
