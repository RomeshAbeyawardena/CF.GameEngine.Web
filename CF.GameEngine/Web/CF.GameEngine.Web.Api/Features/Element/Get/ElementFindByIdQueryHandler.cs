using CF.GameEngine.Infrastructure.Features.Elements;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.Element.Get;

public class ElementFindByIdQueryHandler(IElementRepository elementRepository) 
    : IUnitRequestHandler<ElementFindByIdQuery, ElementResponseDetail>
{
    public async Task<IUnitResult<ElementResponseDetail>> Handle(ElementFindByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await elementRepository.GetElementById(request.ElementId, cancellationToken);
        return result.Convert(x => x.Map<ElementResponseDetail>());
    }
}
