using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.PII;
using IDFCR.Shared.Abstractions.Repositories;
using IDFCR.Shared.Abstractions.Results;
using Microsoft.EntityFrameworkCore;

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
    Task<IUnitResult<CommonNameDto?>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    /// <summary>
    /// This is the raw object, no decryption will be performed
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IUnitResult<DbCommonName?>> GetByNameRawAsync(string name, CancellationToken cancellationToken = default);
}

internal class CommonNameRepository(ICommonNamePIIProtection commonNamePIIProtection, TimeProvider timeProvider, CFIdentityDbContext context) 
    : RepositoryBase<ICommonName, DbCommonName, CommonNameDto>(timeProvider, context), ICommonNameRepository
{
    protected async Task<IUnitResult<DbCommonName>> UpsertAsync(DbCommonName commonName, CancellationToken cancellationToken)
    {
        var result = await base.UpsertAsync(commonName.Map<CommonNameDto>(), cancellationToken);

        if (result.IsSuccess)
        {
            return UnitResult.FromResult(base.LastAddedEntry?.Entity);
        }

        return result.As<DbCommonName>();
    }

    protected override void OnAdd(DbCommonName db, CommonNameDto source)
    {
        commonNamePIIProtection.Protect(db);
        
        base.OnAdd(db, source);
    }

    Task<IUnitResult<DbCommonName>> ICommonNameRepository.UpsertAsync(DbCommonName commonName, CancellationToken cancellationToken)
    {
        return UpsertAsync(commonName, cancellationToken);
    }

    public Task<IUnitResult<CommonNameDto?>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IUnitResult<DbCommonName?>> GetByNameRawAsync(string name, CancellationToken cancellationToken = default)
    {
        commonNamePIIProtection.HashWithHMAC(x => x);
        Context.CommonNames.AsNoTracking()
    }
}

