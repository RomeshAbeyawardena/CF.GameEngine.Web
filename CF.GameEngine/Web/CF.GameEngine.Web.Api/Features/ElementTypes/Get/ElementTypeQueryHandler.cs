using CF.GameEngine.Infrastructure.Features.ElementTypes;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Get;

public class ElementTypeQueryHandler(IElementTypeRepository elementTypeRepository) : IUnitPagedRequestHandler<ElementTypeQuery, ElementTypeResponse>
{
    public async Task<IUnitPagedResult<ElementTypeResponse>> Handle(ElementTypeQuery request, CancellationToken cancellationToken)
    {
        var result = await elementTypeRepository.GetPagedAsync(request, cancellationToken);
        
        return result.Convert(x => x.Map<ElementTypeResponse>());
    }
}