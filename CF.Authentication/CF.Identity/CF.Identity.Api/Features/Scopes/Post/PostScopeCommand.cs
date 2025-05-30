using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Scopes.Post;

public record PostScopeCommand(EditableScopeDto Scope, bool Bypass = false) : IUnitRequest<Guid>, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [SystemRoles.GlobalWrite, ScopeRoles.ScopeWrite];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
