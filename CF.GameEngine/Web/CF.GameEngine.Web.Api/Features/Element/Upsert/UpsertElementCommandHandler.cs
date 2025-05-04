using CF.GameEngine.Infrastructure.Features.Elements;
using CF.GameEngine.Web.Api.Features.ElementTypes;
using CF.GameEngine.Web.Api.Features.ElementTypes.Get;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Exceptions;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.Element.Upsert;

public class UpsertElementCommandHandler(IElementRepository elementRepository, IMediator mediator) : IUnitRequestHandler<UpsertElementCommand, Guid>
{
    public async Task<IUnitResult<Guid>> Handle(UpsertElementCommand request, CancellationToken cancellationToken)
    {
        if(request.Element.ElementTypeId == default && string.IsNullOrWhiteSpace(request.Element.ElementType))
        {
            return new UnitResult(new InvalidEntityStateException("Element", "ElementTypeId or ElementType must be provided.")).As<Guid>();
        }

        var elementTypes = await mediator.Send(new ElementTypeQuery(null, request.Element.ElementType!, null, 1, 1), cancellationToken);

        var elementType = elementTypes.Result?.FirstOrDefault();

        if (elementType is null)
        {
            return new UnitResult(new EntityNotFoundException(typeof(ElementTypeDto), request.Element.ElementType!)).As<Guid>();
        }

        request.Element.ElementTypeId = elementType.Id;

        var result = await elementRepository.UpsertAsync(request.Element.Map<Infrastructure.Features.Elements.ElementDto>(), cancellationToken);

        await elementRepository.SaveChangesAsync(cancellationToken);

        return result;
    }
}