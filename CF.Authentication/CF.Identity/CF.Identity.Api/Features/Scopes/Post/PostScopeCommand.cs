using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions;
using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;
using IDFCR.Shared.Abstractions.Roles.Records;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Scopes.Post;

public record PostScopeCommand(EditableScopeDto Scope, bool Bypass = false) 
    : RoleRequirementBase(() => RoleRegistrar.List<ScopeRoles>(RoleCategory.Write)), IUnitRequest<Guid>;