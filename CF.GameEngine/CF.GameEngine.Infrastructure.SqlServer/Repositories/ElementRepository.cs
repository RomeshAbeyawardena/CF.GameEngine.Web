using CF.GameEngine.Infrastructure.Features.Elements;
using CF.GameEngine.Infrastructure.SqlServer.Models;

namespace CF.GameEngine.Infrastructure.SqlServer.Repositories;

internal class ElementRepository(TimeProvider timeProvider, CFGameEngineDbContext context) 
    : RepositoryBase<IElement, Element, ElementDto>(timeProvider, context), IElementRepository
{
}
