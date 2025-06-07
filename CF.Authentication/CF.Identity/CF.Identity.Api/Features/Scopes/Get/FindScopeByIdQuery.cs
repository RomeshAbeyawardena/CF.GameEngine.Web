using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Results;
using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;
using IDFCR.Shared.Abstractions.Roles.Records;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Scopes.Get;

public record FindScopeByIdQuery(Guid ScopeId, bool Bypass = false) : RoleRequirementBase(() => RoleRegistrar.List<ScopeRoles>(RoleCategory.Read)), IUnitRequest<ScopeDto>;

public class FindScopeByIdQueryHandler(IScopeRepository scopeRepository) : IUnitRequestHandler<FindScopeByIdQuery, ScopeDto>
{
    public async Task<IUnitResult<ScopeDto>> Handle(FindScopeByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await scopeRepository.GetScopeByIdAsync(request.ScopeId, cancellationToken);

        return result.Convert(x => x.Map<ScopeDto>());
    }
}