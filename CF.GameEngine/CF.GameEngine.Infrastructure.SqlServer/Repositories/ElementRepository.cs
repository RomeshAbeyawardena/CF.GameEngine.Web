using CF.GameEngine.Infrastructure.Features.Elements;
using CF.GameEngine.Infrastructure.SqlServer.Filters;
using CF.GameEngine.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Results;

namespace CF.GameEngine.Infrastructure.SqlServer.Repositories;

internal class ElementRepository(TimeProvider timeProvider, CFGameEngineDbContext context)
    : RepositoryBase<IElement, Element, ElementDto>(timeProvider, context), IElementRepository
{
    public async Task<IUnitResult<ElementDto>> GetElementById(Guid elementId, CancellationToken cancellationToken)
    {
        var element = await FindAsync(cancellationToken, [elementId]);
        if (element is null)
        {
            return UnitResult.NotFound<ElementDto>(elementId);
        }

        return new UnitResult<ElementDto>(element, UnitAction.Get);
    }

    public Task<IUnitPagedResult<ElementDto>> GetPagedAsync(IElementPagedFilter pagedQuery, CancellationToken cancellationToken = default)
    {
        var query = new ElementFilter(pagedQuery);
        return base.GetPagedAsync(pagedQuery, new EntityOrder(pagedQuery, "SortOrder"),
            base.Set<Element>(pagedQuery).Where(query.ApplyFilter(Builder, pagedQuery)),
            cancellationToken);
    }
}
