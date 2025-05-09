using CF.GameEngine.Infrastructure.Features.Elements;
using CF.GameEngine.Web.Api.Features.Element.Get;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Exceptions;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.Element.Delete;

public class DeleteElementCommandHandler(IMediator mediator, IElementRepository elementRepository) : IUnitRequestHandler<DeleteElementCommand>
{
    public async Task<IUnitResult> Handle(DeleteElementCommand request, CancellationToken cancellationToken)
    {
        var element = await mediator.Send(new ElementFindByIdQuery(request.ElementId), cancellationToken);

        if (!element.HasValue)
        {
            return new UnitResult(new EntityNotFoundException(typeof(ElementDto), request.ElementId));
        }

        return await elementRepository.DeleteAsync([request.ElementId], cancellationToken);
    }
}