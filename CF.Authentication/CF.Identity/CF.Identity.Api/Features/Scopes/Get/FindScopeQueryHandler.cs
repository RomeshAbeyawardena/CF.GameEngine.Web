using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Scopes.Get;

public class FindScopeQueryHandler(IScopeRepository repository)
    : IUnitRequestCollectionHandler<FindScopeQuery, ScopeDto>
{
    public async Task<IUnitResultCollection<ScopeDto>> Handle(FindScopeQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetScopesAsync(request, cancellationToken);
        return result.Convert(x => x.Map<ScopeDto>());
    }
}
