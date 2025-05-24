using CF.Identity.Infrastructure.Features.Roles;
using CF.Identity.Infrastructure.SqlServer.Models;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal class RoleRepository(TimeProvider timeProvider, CFIdentityDbContext context) 
    : RepositoryBase<IRole, DbRole, RoleDto>(timeProvider, context)
{
}
