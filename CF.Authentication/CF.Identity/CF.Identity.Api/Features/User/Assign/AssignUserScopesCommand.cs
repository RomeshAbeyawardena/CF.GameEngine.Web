using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.Scope;
using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Http.Authentication;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.User.Assign;

public record AssignUserScopesCommand(Guid ClientId, Guid UserId, IEnumerable<string> Scopes, bool Bypass = false) : IUnitRequest, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [ScopeRoles.ScopeRead, UserRoles.UserWrite];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
