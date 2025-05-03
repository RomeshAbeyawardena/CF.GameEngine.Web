using IDFCR.Shared.Abstractions.Results;

namespace IDFCR.Shared.Abstractions.Repositories;

public interface IRepository<T> : IUnitOfWork
{
    Task<IUnitResult<Guid>> UpsertAsync(T value, CancellationToken cancellationToken);
    Task<T?> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
}
