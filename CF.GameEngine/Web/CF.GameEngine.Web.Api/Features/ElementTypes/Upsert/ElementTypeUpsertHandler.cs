using CF.GameEngine.Infrastructure.Features.ElementTypes;
using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Upsert;

public class ElementTypeUpsertHandler(IElementTypeRepository elementTypeRepository) : IRequestHandler<ElementTypeUpsertCommand, IUnitResult<Guid>>
{
    public async Task<IUnitResult<Guid>> Handle(ElementTypeUpsertCommand request, CancellationToken cancellationToken)
    {
        var result = await elementTypeRepository.UpsertAsync(request.ElementType.Map<Infrastructure.Features.ElementTypes.ElementTypeDto>(), cancellationToken);

        await elementTypeRepository.SaveChangesAsync(cancellationToken);

        return result;
    }
}
