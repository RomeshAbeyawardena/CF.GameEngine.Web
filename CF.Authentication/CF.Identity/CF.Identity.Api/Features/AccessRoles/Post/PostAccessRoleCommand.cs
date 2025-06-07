using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.AccessRoles.Post;

public record PostAccessRoleCommand(EditableAccessRoleDto AccessRole, bool Bypass = false) : IUnitRequest<Guid>, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => RoleRegistrar.List<Roles>(IDFCR.Shared.Abstractions.RoleCategory.Write);
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
