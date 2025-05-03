using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.ElementTypes;

public class ElementTypeDeleteCommandHandler<IElement>() : IRequestHandler<ElementTypeDeleteCommand, IUnitResult>
{
    public Task<IUnitResult> Handle(ElementTypeDeleteCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
