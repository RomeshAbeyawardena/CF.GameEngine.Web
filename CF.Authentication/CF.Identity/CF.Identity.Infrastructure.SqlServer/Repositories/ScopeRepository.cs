﻿using CF.Identity.Infrastructure.Features.Scope;
using CF.Identity.Infrastructure.SqlServer.Filters;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Results;
using Microsoft.EntityFrameworkCore;


namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal class ScopeRepository(TimeProvider timeProvider, CFIdentityDbContext context)
    : RepositoryBase<IScope, DbScope, ScopeDto>(timeProvider, context), IScopeRepository
{
    public Task<IUnitPagedResult<ScopeDto>> GetPagedScopesAsync(IPagedScopeFilter filter, CancellationToken cancellation)
    {
        var query = new ScopeFilter(filter);

        return GetPagedAsync(filter,
            new EntityOrder(filter, nameof(ScopeDto.Name)),
            Set<DbScope>(filter).Where(query.ApplyFilter(Builder)),
            cancellation);
    }

    public async Task<IUnitResult<ScopeDto>> GetScopeByIdAsync(Guid scopeId, CancellationToken cancellationToken)
    {
        var scope = await FindAsync(cancellationToken, [scopeId]);

        if (scope is null)
        {
            return UnitResult.NotFound<ScopeDto>(scopeId);
        }

        return UnitResult.FromResult(scope);
    }

    public async Task<IUnitResultCollection<ScopeDto>> GetScopesAsync(IScopeFilter filter, CancellationToken cancellationToken)
    {
        var result = await Set<DbScope>(filter)
            .Where(new ScopeFilter(filter).ApplyFilter(Builder))
            .ToListAsync(cancellationToken);

        return UnitResultCollection.FromResult(MapTo(result), UnitAction.Get);
    }
}
