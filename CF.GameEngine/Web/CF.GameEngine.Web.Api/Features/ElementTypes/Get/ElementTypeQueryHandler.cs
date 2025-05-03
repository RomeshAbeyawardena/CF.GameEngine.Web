using CF.GameEngine.Infrastructure.Features.ElementTypes;
using IDFCR.Shared.Abstractions.Results;
using MediatR;
using IDFCR.Shared.Extensions;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Get;

public class ElementTypeQueryHandler(IElementTypeRepository elementTypeRepository) : IRequestHandler<ElementTypeQuery, IUnitPagedResult<ElementTypeResponse>>
{
    public async Task<IUnitPagedResult<ElementTypeResponse>> Handle(ElementTypeQuery request, CancellationToken cancellationToken)
    {
        var result = await elementTypeRepository.GetPagedAsync(request, cancellationToken);
        
        return result.Convert(x => x.Map<ElementTypeResponse>());
    }
}