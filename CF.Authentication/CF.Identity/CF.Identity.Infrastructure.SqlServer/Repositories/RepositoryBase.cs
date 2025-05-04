using IDFCR.Shared.Abstractions;
using IDFCR.Shared.EntityFramework;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal abstract class RepositoryBase<TAbstraction, TDb, T>(TimeProvider timeProvider, CFIdentityDbContext context) 
    : RepositoryBase<CFIdentityDbContext, TAbstraction, TDb, T>(timeProvider, context)
     where T : class, IMappable<TAbstraction>, IIdentifer
    where TDb : class, IMappable<TAbstraction>, IIdentifer
{
    
}
