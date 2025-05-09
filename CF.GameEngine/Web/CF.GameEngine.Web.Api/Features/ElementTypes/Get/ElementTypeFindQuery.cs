using CF.GameEngine.Infrastructure.Features.ElementTypes;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Get;

public record ElementTypeFindQuery(string? ExternalReference = null, string? Key = null, string? NameContains = null, bool NoTracking = true) 
    : IUnitRequestCollection<ElementTypeDto>, IElementTypeFilter;

public class ElementTypeFindQueryHandler(IElementTypeRepository elementTypeRepository) : IUnitRequestCollectionHandler<ElementTypeFindQuery, ElementTypeDto>
{
    public async Task<IUnitResultCollection<ElementTypeDto>> Handle(ElementTypeFindQuery request, CancellationToken cancellationToken)
    {
        var results = await elementTypeRepository.FindElementTypesAsync(request, cancellationToken);

        return results.Convert(x => x.Map<ElementTypeDto>());
    }
}