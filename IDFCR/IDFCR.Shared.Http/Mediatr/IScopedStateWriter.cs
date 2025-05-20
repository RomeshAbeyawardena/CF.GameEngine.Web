using IDFCR.Shared.Abstractions.Results;

namespace IDFCR.Shared.Http.Mediatr
{
    public interface IScopedStateWriter
    {
        Task<IUnitResult> WriteAsync(string key, object value, CancellationToken cancellationToken);
    }
}
