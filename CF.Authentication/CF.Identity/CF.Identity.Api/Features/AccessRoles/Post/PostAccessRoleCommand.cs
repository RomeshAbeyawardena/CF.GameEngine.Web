using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.AccessRoles.Post;

public record PostAccessRoleCommand(EditableAccessRoleDto AccessRole, bool Bypass = false) : IUnitRequest<Guid>, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [SystemRoles.GlobalWrite, Roles.RoleWrite];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
