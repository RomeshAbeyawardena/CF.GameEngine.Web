using CF.GameEngine.Infrastructure.Features.ElementTypes;
using CF.GameEngine.Infrastructure.SqlServer.Models;

namespace CF.GameEngine.Infrastructure.SqlServer.Repositories;

internal class ElementTypeRepository(TimeProvider timeProvider, CFGameEngineDbContext context)
    : RepositoryBase<IElementType, ElementType, ElementTypeDto>(timeProvider, context), 
    IElementTypeRepository
{
}
