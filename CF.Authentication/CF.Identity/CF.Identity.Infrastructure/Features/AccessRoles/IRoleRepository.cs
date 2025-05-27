using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions.Repositories;

namespace CF.Identity.Infrastructure.Features.Roles;

public interface IAccessRoleRepository : IRepository<AccessRoleDto>
{
}
