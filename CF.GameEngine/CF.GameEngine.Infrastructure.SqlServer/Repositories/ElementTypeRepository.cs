using CF.GameEngine.Infrastructure.Features.ElementTypes;
using CF.GameEngine.Infrastructure.SqlServer.Filters;
using CF.GameEngine.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Results;
using Microsoft.EntityFrameworkCore;

namespace CF.GameEngine.Infrastructure.SqlServer.Repositories;

internal class ElementTypeRepository(TimeProvider timeProvider, CFGameEngineDbContext context)
    : RepositoryBase<IElementType, ElementType, ElementTypeDto>(timeProvider, context),
    IElementTypeRepository
{
    public async Task<IUnitResult<ElementTypeDto>> GetElementTypeById(Guid elementTypeId, CancellationToken cancellationToken)
    {
        var elementType = await FindAsync(cancellationToken, [elementTypeId]);

        if (elementType == null)
        {
            return UnitResult.NotFound<ElementTypeDto>(elementTypeId);
        }

        return UnitResult.FromResult(elementType, UnitAction.Get);
    }

    public async Task<IUnitResultCollection<ElementTypeDto>> FindElementTypesAsync(IElementTypeFilter filter, CancellationToken cancellationToken)
    {
        var query = new ElementTypeFilter(filter);
        var elementTypes = await base.Set<ElementType>(filter)
            .Where(query.ApplyFilter(Builder))
            .ToListAsync(cancellationToken);

        return UnitResultCollection.FromResult(elementTypes.Select(x => x.Map<ElementTypeDto>()).ToList(), UnitAction.Get);
    }

    public Task<IUnitPagedResult<ElementTypeDto>> GetPagedAsync(IElementTypePagedFilter pagedQuery, CancellationToken cancellationToken)
    {
        var query = new ElementTypeFilter(pagedQuery);
        return base.GetPagedAsync(pagedQuery, new EntityOrder(pagedQuery, "SortOrder"),
            base.Set<ElementType>(pagedQuery).Where(query.ApplyFilter(Builder)), 
            cancellationToken);
    }
}
