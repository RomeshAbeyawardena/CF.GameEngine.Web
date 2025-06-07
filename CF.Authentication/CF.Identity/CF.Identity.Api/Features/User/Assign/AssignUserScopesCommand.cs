using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.User.Assign;

public record AssignUserScopesCommand(Guid ClientId, Guid UserId, IEnumerable<string> Scopes, bool Bypass = false) : IUnitRequest, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => RoleRegistrar.List<UserRoles>(RoleCategory.Write);
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
