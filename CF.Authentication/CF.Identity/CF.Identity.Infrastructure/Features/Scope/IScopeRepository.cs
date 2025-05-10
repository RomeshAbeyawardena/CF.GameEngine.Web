using IDFCR.Shared.Abstractions.Repositories;
using IDFCR.Shared.Abstractions.Results;

namespace CF.Identity.Infrastructure.Features.Scope;

public interface IScopeRepository : IRepository<ScopeDto>
{
    Task<IUnitResultCollection<ScopeDto>> GetScopesAsync(IScopeFilter filter, CancellationToken cancellationToken);
}
