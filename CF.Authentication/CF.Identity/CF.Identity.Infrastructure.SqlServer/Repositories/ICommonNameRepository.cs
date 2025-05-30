using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.PII;
using IDFCR.Shared.Abstractions.Repositories;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
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
    Task<IUnitResult<CommonNameDto>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    /// <summary>
    /// This is the raw object, no decryption will be performed
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IUnitResult<DbCommonName>> GetByNameRawAsync(string name, bool needsTracking, CancellationToken cancellationToken = default);
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

    public async Task<IUnitResult<CommonNameDto>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        //we're not tracking this because it brings risk we may accidentally update the database with unencrypted values
        var result = await GetByNameRawAsync(name, false, cancellationToken);
            
        var commonName = result.GetResultOrDefault();

        if(commonName is null)
        {
            return result.As<CommonNameDto>();
        }

        commonNamePIIProtection.Unprotect(commonName);
        return UnitResult.FromResult(commonName.Map<CommonNameDto>());
    }

    public async Task<IUnitResult<DbCommonName>> GetByNameRawAsync(string name, bool needsTracking = false, CancellationToken cancellationToken = default)
    {
        var hmacValue = commonNamePIIProtection.HashWithHmac(name);

        IQueryable<DbCommonName> query = needsTracking ? Context.CommonNames : Context.CommonNames.AsNoTracking();

        var result = await query.Where(x => x.ValueHmac == hmacValue).FirstOrDefaultAsync(cancellationToken);

        if (result is null)
        {
            return UnitResult.NotFound<DbCommonName>(name);
        }

        return UnitResult.FromResult(result);
    }
}

