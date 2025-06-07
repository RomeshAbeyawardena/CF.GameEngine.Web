using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions;
using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;
using IDFCR.Shared.Abstractions.Roles.Records;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Scopes.Get;

public record FindScopesQuery(Guid? ClientId = null,
        Guid? UserId = null,
        string? Key = null,
        IEnumerable<string>? Keys = null,
        bool IncludePrivilegedScoped = false,
        bool Bypass = false,
        bool NoTracking = true) : RoleRequirementBase(() => RoleRegistrar.List<ScopeRoles>(RoleCategory.Read)),
    IUnitRequestCollection<ScopeDto>, IScopeFilter;
