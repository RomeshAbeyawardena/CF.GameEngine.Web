using CF.GameEngine.Infrastructure.Features.ElementTypes;
using CF.GameEngine.Web.Api.Features.ElementTypes.Get;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Exceptions;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Delete;

public class ElementTypeDeleteCommandHandler<IElement>(IElementTypeRepository elementTypeRepository, IMediator mediator) 
    : IUnitRequestHandler<ElementTypeDeleteCommand>
{
    public async Task<IUnitResult> Handle(ElementTypeDeleteCommand request, CancellationToken cancellationToken)
    {
        var elementType = await mediator.Send(new ElementTypeFindByIdQuery(request.ElementTypeId), cancellationToken);
        
        if (!elementType.HasValue)
        {
            return new UnitResult(new EntityNotFoundException(typeof(ElementTypeDto), request.ElementTypeId));
        }

        return await elementTypeRepository.DeleteAsync([request.ElementTypeId], cancellationToken);
    }
}
