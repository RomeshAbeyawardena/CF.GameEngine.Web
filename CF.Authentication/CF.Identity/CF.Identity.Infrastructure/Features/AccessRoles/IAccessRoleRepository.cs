using IDFCR.Shared.Abstractions.Repositories;
using IDFCR.Shared.Abstractions.Results;

namespace CF.Identity.Infrastructure.Features.AccessRoles;

public interface IAccessRoleRepository : IRepository<AccessRoleDto>
{
    Task<IUnitPagedResult<AccessRoleDto>> ListRolesAsync(IPagedAccessRoleFilter filter, CancellationToken cancellationToken);
    Task<IUnitResultCollection<AccessRoleDto>> GetAccessRolesAsync(IAccessRoleFilter filter, CancellationToken cancellationToken);
    Task<IUnitResult<AccessRoleDto>> FindAccessTokenAsync(Guid id, CancellationToken cancellationToken);
}
