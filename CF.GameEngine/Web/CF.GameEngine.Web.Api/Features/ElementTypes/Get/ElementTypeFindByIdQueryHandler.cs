using CF.GameEngine.Infrastructure.Features.ElementTypes;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Get;

public class ElementTypeFindByIdQueryHandler(IElementTypeRepository elementTypeRepository) : IUnitRequestHandler<ElementTypeFindByIdQuery, ElementTypeResponseDetail>
{
    public async Task<IUnitResult<ElementTypeResponseDetail>> Handle(ElementTypeFindByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await elementTypeRepository.GetElementTypeById(request.ElementTypeId, cancellationToken);
            
        return result.Convert(x => x.Map<ElementTypeResponseDetail>());
    }
}