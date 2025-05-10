using CF.Identity.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CF.Identity.Infrastructure.SqlServer;

internal class CFIdentityDbContext(DbContextOptions<CFIdentityDbContext> options) : DbContext(options)
{
    public DbSet<DbAccessToken> AccessTokens { get; set; } = null!;
    public DbSet<DbClient> Clients { get; set; } = null!;
    public DbSet<DbScope> Scopes { get; set; } = null!;
}
