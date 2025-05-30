using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Mediatr;

using IRoleRequirement = IDFCR.Shared.Abstractions.Roles.IRoleRequirement;
using RoleRequirementType = IDFCR.Shared.Abstractions.Roles.RoleRequirementType;

namespace CF.Identity.Api.Features.Scopes.Get;

public record FindScopesQuery(Guid? ClientId = null,
        Guid? UserId = null,
        string? Key = null,
        IEnumerable<string>? Keys = null,
        bool IncludePrivilegedScoped = false,
        bool Bypass = false,
        bool NoTracking = true) :
    IUnitRequestCollection<ScopeDto>, IScopeFilter, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [SystemRoles.GlobalRead, ScopeRoles.ScopeRead];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
