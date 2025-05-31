using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Repositories;
using IDFCR.Shared.Abstractions.Results;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

public interface ICommonNameRepository : IRepository<CommonNameDto>
{
    /// <summary>
    /// A proxy for the UpsertAsync method that returns the actual tracked DbCommonName object.
    /// </summary>
    /// <param name="commonName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IUnitResult<DbCommonName>> UpsertAsync(DbCommonName commonName, CancellationToken cancellationToken);
    Task<IUnitResult<CommonNameDto>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    /// <summary>
    /// This is the raw object, no decryption will be performed
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IUnitResult<DbCommonName>> GetByNameRawAsync(string name, bool needsTracking, CancellationToken cancellationToken = default);
    /// <summary>
    /// This is the raw object, no decryption will be performed
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IUnitResult<DbCommonName>> GetAnonymisedRowRawAsync(bool needsTracking = false, CancellationToken cancellationToken = default);
}

