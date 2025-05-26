using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Scopes.Get;

public class FindScopeQueryHandler(IScopeRepository repository)
    : IUnitRequestCollectionHandler<FindScopesQuery, ScopeDto>
{
    public async Task<IUnitResultCollection<ScopeDto>> Handle(FindScopesQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetScopesAsync(request, cancellationToken);
        return result.Convert(x => x.Map<ScopeDto>());
    }
}
