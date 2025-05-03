using IDFCR.Shared.Abstractions;
using IDFCR.Shared.EntityFramework;

namespace CF.GameEngine.Infrastructure.SqlServer.Repositories;

internal class RepositoryBase<TAbstraction, TDb, T>(TimeProvider timeProvider,
    CFGameEngineDbContext context) 
    : RepositoryBase<CFGameEngineDbContext,TAbstraction, TDb, T>(timeProvider, context)
    where T : class, IMappable<TAbstraction>, IIdentifer
    where TDb : class, IMappable<TAbstraction>, IIdentifer
{

}
