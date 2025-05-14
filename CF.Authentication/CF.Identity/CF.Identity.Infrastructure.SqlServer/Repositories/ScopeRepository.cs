using CF.Identity.Infrastructure.Features.Scope;
using CF.Identity.Infrastructure.SqlServer.Filters;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Results;
using Microsoft.EntityFrameworkCore;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal class ScopeRepository(TimeProvider timeProvider, CFIdentityDbContext context)
    : RepositoryBase<IScope, DbScope, ScopeDto>(timeProvider, context), IScopeRepository
{
    public async Task<IUnitResultCollection<ScopeDto>> GetScopesAsync(IScopeFilter filter, CancellationToken cancellationToken)
    {
        var result = await Set<DbScope>(filter)
            .Where(new ScopeFilter(filter).ApplyFilter(Builder))
            .ToListAsync(cancellationToken);

        return UnitResultCollection.FromResult(MapTo(result), UnitAction.Get);
    }
}
