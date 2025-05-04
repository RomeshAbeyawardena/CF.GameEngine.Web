using Microsoft.EntityFrameworkCore;

namespace CF.Identity.Infrastructure.SqlServer;

public class CFIdentityDbContext(DbContextOptions<CFIdentityDbContext> options) : DbContext(options)
{

}
