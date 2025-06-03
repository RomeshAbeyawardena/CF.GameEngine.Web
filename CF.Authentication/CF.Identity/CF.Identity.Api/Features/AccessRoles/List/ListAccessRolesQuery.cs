using CF.Identity.Infrastructure.Features.AccessRoles;
using CF.Identity.Infrastructure.Features;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.AccessRoles.List;

public record ListAccessRolesQuery(bool Bypass = false) : IUnitPagedRequest<AccessRoleDto>, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [SystemRoles.GlobalWrite, Roles.RoleRead];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
