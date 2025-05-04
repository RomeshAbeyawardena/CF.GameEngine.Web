using CF.GameEngine.Web.Api.Features.Element.Upsert;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Exceptions;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.Element.Put;

public class PutElementCommandHandler(IMediator mediator) : IUnitRequestHandler<PutElementCommand, Guid>
{
    public async Task<IUnitResult<Guid>> Handle(PutElementCommand request, CancellationToken cancellationToken)
    {
        if(request.Element.Id == default)
        {
            return new UnitResult(new InvalidEntityStateException("Element", "Element ID must be provided for update.")).As<Guid>();
        }

        return await mediator.Send(new UpsertElementCommand(request.Element), cancellationToken);
    }
}