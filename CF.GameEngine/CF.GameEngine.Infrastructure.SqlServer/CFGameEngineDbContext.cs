using Microsoft.EntityFrameworkCore;

namespace CF.GameEngine.Infrastructure.SqlServer;

internal class CFGameEngineDbContext(DbContextOptions<CFGameEngineDbContext> options) : DbContext(options)
{
}
