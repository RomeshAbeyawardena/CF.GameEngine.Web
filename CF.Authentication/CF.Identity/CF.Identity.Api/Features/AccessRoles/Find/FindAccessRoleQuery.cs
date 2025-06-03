using CF.Identity.Infrastructure.Features.AccessRoles;
using CF.Identity.Infrastructure.Features;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.AccessRoles.Find;

public record FindAccessRoleQuery(Guid AccessRoleId, bool Bypass = false) : IUnitRequest<AccessRoleDetail>, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [SystemRoles.GlobalRead, Roles.RoleRead];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
