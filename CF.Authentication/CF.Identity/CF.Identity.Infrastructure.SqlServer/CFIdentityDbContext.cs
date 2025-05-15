using CF.Identity.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CF.Identity.Infrastructure.SqlServer;

public class CFIdentityDbContext(DbContextOptions<CFIdentityDbContext> options) : DbContext(options)
{
    public DbSet<DbAccessToken> AccessTokens { get; set; } = null!;
    public DbSet<DbClient> Clients { get; set; } = null!;
    public DbSet<DbScope> Scopes { get; set; } = null!;
    public DbSet<DbUser> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CFIdentityDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
