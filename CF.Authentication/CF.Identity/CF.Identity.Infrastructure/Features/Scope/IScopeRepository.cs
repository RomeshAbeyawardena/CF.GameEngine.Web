using IDFCR.Shared.Abstractions.Repositories;
using IDFCR.Shared.Abstractions.Results;

namespace CF.Identity.Infrastructure.Features.Scope;

public interface IScopeRepository : IRepository<ScopeDto>
{
    Task<IUnitPagedResult<ScopeDto>> GetPagedScopesAsync(IPagedScopeFilter filter,
        CancellationToken cancellation);
    Task<IUnitResultCollection<ScopeDto>> GetScopesAsync(IScopeFilter filter, CancellationToken cancellationToken);
}
