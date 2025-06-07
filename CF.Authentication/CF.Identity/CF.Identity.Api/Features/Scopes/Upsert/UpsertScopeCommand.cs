using CF.Identity.Api.Features.Scopes.Post;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Scopes.Upsert;

public record UpsertScopeCommand(EditableScopeDto Scope, bool Bypass = false) : IUnitRequest<Guid>, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => RoleRegistrar.List<ScopeRoles>(RoleCategory.Write);
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}

public class UpsertScopeCommandHandler(IScopeRepository scopeRepository)
    : IUnitRequestHandler<UpsertScopeCommand, Guid>
{
    public Task<IUnitResult<Guid>> Handle(UpsertScopeCommand request, CancellationToken cancellationToken)
    {
        return scopeRepository.UpsertAsync(request.Scope.Map<Infrastructure.Features.Scope.ScopeDto>(), cancellationToken);
    }
}
