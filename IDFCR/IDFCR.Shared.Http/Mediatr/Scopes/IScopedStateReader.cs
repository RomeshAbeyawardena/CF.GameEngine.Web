using IDFCR.Shared.Abstractions.Results;

namespace IDFCR.Shared.Http.Mediatr.Scopes
{
    public interface IScopedStateReader
    {
        Task<IUnitResult<IScopedState>> ReadAsync(string key, CancellationToken cancellationToken);
        Task<IUnitResult<IScopedState<T>>> ReadAsync<T>(string key, CancellationToken cancellationToken);
    }
}
