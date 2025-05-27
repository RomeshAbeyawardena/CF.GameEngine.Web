using CF.Identity.Infrastructure.Features.AccessRoles;
using CF.Identity.Infrastructure.Features.Roles;
using CF.Identity.Infrastructure.SqlServer.Models;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal class AccessRoleRepository(TimeProvider timeProvider, CFIdentityDbContext context) 
    : RepositoryBase<IAccessRole, DbAccessRole, AccessRoleDto>(timeProvider, context), IAccessRoleRepository
{
}
