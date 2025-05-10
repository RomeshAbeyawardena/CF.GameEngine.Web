using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.SqlServer.Filters;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Results;
using Microsoft.EntityFrameworkCore;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal class ClientRepository(TimeProvider timeProvider, CFIdentityDbContext context) : RepositoryBase<IClient, DbClient, ClientDto>(timeProvider, context), IClientRepository
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

    public async Task<IUnitResultCollection<ClientDto>> GetClients(IClientFilter filter, CancellationToken cancellationToken)
    {
        var clientFilter = new ClientFilter(filter);

        var result = await Set<DbClient>(filter)
            .Where(clientFilter.ApplyFilter(Builder, filter))
            .ToListAsync(cancellationToken);

        return new UnitResultCollection<ClientDto>([.. result.Select(x => x.Map<ClientDto>())], UnitAction.Get);
    }
}
