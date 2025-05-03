using CF.GameEngine.Web.Api.Features.ElementTypes.Upsert;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Exceptions;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Put;

public class PutElementTypeCommandHandler(IMediator mediator) : IRequestHandler<PutElementTypeCommand, IUnitResult<Guid>>
{
    public async Task<IUnitResult<Guid>> Handle(PutElementTypeCommand request, CancellationToken cancellationToken)
    {
        if (request.ElementType.Id == default)
        {
            return new UnitResult(new InvalidEntityStateException("ElementType", "ElementType ID must not be empty.")).As<Guid>();
        }


        return await mediator.Send(new ElementTypeUpsertCommand(request.ElementType), cancellationToken);
    }
}
