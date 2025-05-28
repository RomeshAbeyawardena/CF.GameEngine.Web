using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.PII;
using IDFCR.Shared.Abstractions.Repositories;
using IDFCR.Shared.Abstractions.Results;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal interface ICommonNameRepository : IRepository<CommonNameDto>
{
    Task<IUnitResult<CommonNameDto>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}

internal class CommonNameRepository(ICommonNamePIIProtection commonNamePIIProtection, TimeProvider timeProvider, CFIdentityDbContext context) 
    : RepositoryBase<ICommonName, DbCommonName, CommonNameDto>(timeProvider, context)
{
    protected override void OnAdd(DbCommonName db, CommonNameDto source)
    {
        commonNamePIIProtection.Protect(db);
        base.OnAdd(db, source);
    }
}

