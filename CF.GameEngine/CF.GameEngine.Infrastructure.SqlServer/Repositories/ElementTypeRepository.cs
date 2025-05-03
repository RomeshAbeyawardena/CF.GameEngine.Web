using CF.GameEngine.Infrastructure.Features.ElementTypes;
using CF.GameEngine.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Exceptions;

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
            return new UnitResult(new EntityNotFoundException("Element", elementTypeId)).As<ElementTypeDto>();
        }

        return new UnitResult<ElementTypeDto>(elementType);
    }
}
