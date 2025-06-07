using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.AccessRoles.Find;

public record FindAccessRoleQuery(Guid AccessRoleId, bool Bypass = false) : IUnitRequest<AccessRoleDetail>, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => RoleRegistrar.List<Roles>(RoleCategory.Read, SystemRoles.GlobalRead);
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
