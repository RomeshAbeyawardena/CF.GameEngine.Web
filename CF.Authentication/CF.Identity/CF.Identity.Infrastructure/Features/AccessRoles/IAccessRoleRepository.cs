using IDFCR.Shared.Abstractions.Repositories;
using IDFCR.Shared.Abstractions.Results;

namespace CF.Identity.Infrastructure.Features.AccessRoles;

public interface IAccessRoleRepository : IRepository<AccessRoleDto>
{
    Task<IUnitResultCollection<AccessRoleDto>> GetAccessRolesAsync(IAccessRoleFilter filter, CancellationToken cancellationToken);
}
