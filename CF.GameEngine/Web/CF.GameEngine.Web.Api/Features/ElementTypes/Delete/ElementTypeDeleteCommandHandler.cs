using CF.GameEngine.Infrastructure.Features.ElementTypes;
using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Delete;

public class ElementTypeDeleteCommandHandler<IElement>(IElementTypeRepository elementTypeRepository) : IRequestHandler<ElementTypeDeleteCommand, IUnitResult>
{
    public Task<IUnitResult> Handle(ElementTypeDeleteCommand request, CancellationToken cancellationToken)
    {
        return elementTypeRepository.DeleteAsync([request.ElementTypeId], cancellationToken);
    }
}
