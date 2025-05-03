using IDFCR.Shared.Abstractions.Repositories;
using IDFCR.Shared.Abstractions.Results;

namespace CF.GameEngine.Infrastructure.Features.ElementTypes;

public interface IElementTypeRepository : IRepository<ElementTypeDto>
{
    Task<IUnitResult<ElementTypeDto>> GetElementTypeById(Guid elementTypeId, CancellationToken cancellationToken);
}
