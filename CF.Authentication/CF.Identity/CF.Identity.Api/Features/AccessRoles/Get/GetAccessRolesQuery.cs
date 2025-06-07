using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.AccessRoles.Get;

public record GetAccessRolesQuery(Guid? ClientId = null, string? Name = null, string? NameContains = null,
    bool NoTracking = true, bool Bypass = false) : IUnitRequestCollection<AccessRoleDto>, IAccessRoleFilter, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => RoleRegistrar.List<Roles>(RoleCategory.Read);
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
