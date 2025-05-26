using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Scopes.Get;

public class GetPagedScopesQueryHandler(IScopeRepository scopeRepository) : IUnitPagedRequestHandler<GetPagedScopesQuery, ScopeDto>
{
    public async Task<IUnitPagedResult<ScopeDto>> Handle(GetPagedScopesQuery request, CancellationToken cancellationToken)
    {
        var scopes = await scopeRepository.GetPagedScopesAsync(request, cancellationToken);
        
        return scopes.Convert(e => e.Map<ScopeDto>());
    }
}