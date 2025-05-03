using CF.GameEngine.Infrastructure.Features.ElementTypes;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Get;

public class ElementTypeFindQueryHandler(IElementTypeRepository elementTypeRepository) : IRequestHandler<ElementTypeFindQuery, IUnitResult<ElementTypeResponse>>
{
    public async Task<IUnitResult<ElementTypeResponse>> Handle(ElementTypeFindQuery request, CancellationToken cancellationToken)
    {
        var result = await elementTypeRepository.GetElementTypeById(request.ElementTypeId, cancellationToken);
            
        return result.Convert(x => x.Map<ElementTypeResponse>());
    }
}