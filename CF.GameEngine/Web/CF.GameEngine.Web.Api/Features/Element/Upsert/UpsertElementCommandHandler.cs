using CF.GameEngine.Infrastructure.Features.Elements;
using CF.GameEngine.Web.Api.Features.Element.Get;
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
        if(!request.Element.ElementTypeId.HasValue && string.IsNullOrWhiteSpace(request.Element.ElementType))
        {
            return new UnitResult(new InvalidEntityStateException(typeof(ElementDto), "ElementTypeId or ElementType must be provided.")).As<Guid>();
        }
        
        if (request.Element.ElementTypeId.HasValue)
        {
            var foundElementType = await mediator.Send(new ElementTypeFindByIdQuery(request.Element.ElementTypeId.Value), cancellationToken);
            
            if(foundElementType.Result is null)
            {
                return new UnitResult(new EntityNotFoundException(typeof(ElementTypeDto), request.Element.ElementTypeId.Value)).As<Guid>();
            }
        }
        else if (!string.IsNullOrWhiteSpace(request.Element.ElementType))
        {
            var elementTypes = await mediator.Send(new ElementTypeFindQuery(Key: request.Element.ElementType), cancellationToken);

            var elementType = elementTypes.Result?.FirstOrDefault();

            if (elementType is null)
            {
                return new UnitResult(new EntityNotFoundException(typeof(ElementTypeDto), request.Element.ElementType!)).As<Guid>();
            }

            request.Element.ElementTypeId = elementType.Id;
        }

        if (!string.IsNullOrWhiteSpace(request.Element.ParentElement))
        {
            var parentElements = await mediator.Send(new ElementQuery(Key: request.Element.ParentElement, PageSize: 1, PageIndex: 0), cancellationToken);
            var parentElement = parentElements.Result?.FirstOrDefault();
            
            if (parentElement is null)
            {
                return new UnitResult(new EntityNotFoundException(typeof(ElementDto), request.Element.ParentElement!)).As<Guid>();
            }

            request.Element.ParentElementId = parentElement.Id;
        }

        var result = await elementRepository.UpsertAsync(request.Element.Map<Infrastructure.Features.Elements.ElementDto>(), cancellationToken);

        await elementRepository.SaveChangesAsync(cancellationToken);

        return result;
    }
}