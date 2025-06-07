using CF.Identity.Api.Features.Scopes.Post;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Results;
using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;
using IDFCR.Shared.Abstractions.Roles.Records;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Scopes.Upsert;

public record UpsertScopeCommand(EditableScopeDto Scope, bool Bypass = false) 
    : RoleRequirementBase(() => RoleRegistrar.List<ScopeRoles>(RoleCategory.Write)), IUnitRequest<Guid>;

public class UpsertScopeCommandHandler(IScopeRepository scopeRepository)
    : IUnitRequestHandler<UpsertScopeCommand, Guid>
{
    public Task<IUnitResult<Guid>> Handle(UpsertScopeCommand request, CancellationToken cancellationToken)
    {
        return scopeRepository.UpsertAsync(request.Scope.Map<Infrastructure.Features.Scope.ScopeDto>(), cancellationToken);
    }
}
