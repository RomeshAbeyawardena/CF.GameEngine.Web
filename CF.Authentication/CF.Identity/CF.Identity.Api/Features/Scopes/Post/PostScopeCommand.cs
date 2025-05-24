using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Scopes.Post;

public record PostScopeCommand(EditableScopeDto Scope, bool Bypass = false) : IUnitRequest<Guid>, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [Roles.GlobalWrite, Roles.ScopeWrite];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
