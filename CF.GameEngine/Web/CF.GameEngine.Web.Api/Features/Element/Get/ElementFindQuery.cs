using CF.GameEngine.Infrastructure.Features.Elements;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.Element.Get;

public record ElementFindQuery(Guid? ParentElementId = null, string? ExternalReference = null, 
        string? Key = null, string? NameContains = null, bool NoTracking = true) : IElementFilter, IUnitRequestCollection<ElementDto>;

public class ElementFindQueryHandler(IElementRepository elementRepository) : IUnitRequestCollectionHandler<ElementFindQuery, ElementDto>
{
    public async Task<IUnitResultCollection<ElementDto>> Handle(ElementFindQuery request, CancellationToken cancellationToken)
    {
        var elements = await elementRepository.FindElementsAsync(request, cancellationToken);
        return elements.Convert(e => e.Map<ElementDto>());
    }
}