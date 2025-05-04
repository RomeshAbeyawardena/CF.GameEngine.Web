using CF.GameEngine.Infrastructure.Features.Elements;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.Element.Get;

public class ElementQueryHandler(IElementRepository elementRepository) 
    : IUnitPagedRequestHandler<ElementQuery, ElementResponse>
{
    
    public async Task<IUnitPagedResult<ElementResponse>> Handle(ElementQuery request, CancellationToken cancellationToken)
    {
        var result = await elementRepository.GetPagedAsync(request, cancellationToken);
        return result.Convert(e => e.Map<ElementResponse>());
    }
}

