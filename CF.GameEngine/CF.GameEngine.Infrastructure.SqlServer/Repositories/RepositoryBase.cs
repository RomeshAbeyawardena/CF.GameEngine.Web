using IDFCR.Shared.Abstractions;
using IDFCR.Shared.EntityFramework;

namespace CF.GameEngine.Infrastructure.SqlServer.Repositories;

internal class RepositoryBase<TAbstraction, TDb, T>(CFGameEngineDbContext context) : RepositoryBase<CFGameEngineDbContext,TAbstraction, TDb, T>(context)
    where T : class, IMappable<TAbstraction>, IIdentifer
    where TDb : class, IMappable<TAbstraction>, IIdentifer
{

}
