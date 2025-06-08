using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Roles.Records;
using IDFCR.Shared.Mediatr;

using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;

namespace CF.Identity.Api.Features.AccessRoles.Get;

public record GetAccessRolesQuery(Guid? ClientId = null, string? Name = null, string? NameContains = null,
    bool NoTracking = true, bool Bypass = false) 
    : RoleRequirementBase(() => RoleRegistrar.List<Roles>(RoleCategory.Read)), 
    IUnitRequestCollection<AccessRoleDto>, IAccessRoleFilter
{
    
}
