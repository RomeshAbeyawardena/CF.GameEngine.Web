using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Scopes.Get;

public record FindScopeQuery(Guid? ClientId = null, Guid? UserId = null, string? Key = null, bool IncludePrivilegedScoped = false,
    IEnumerable<string>? Keys = null, bool NoTracking = true, bool Bypass = false) : IUnitRequestCollection<ScopeDto>, IScopeFilter, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles =>  [SystemRoles.GlobalRead, ScopeRoles.ScopeRead];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
