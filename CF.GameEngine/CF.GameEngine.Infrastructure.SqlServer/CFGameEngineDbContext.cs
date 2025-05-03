using CF.GameEngine.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CF.GameEngine.Infrastructure.SqlServer;

internal class CFGameEngineDbContext(DbContextOptions<CFGameEngineDbContext> options) : DbContext(options)
{
    public DbSet<ElementType> ElementTypes { get; set; } = null!;
}
