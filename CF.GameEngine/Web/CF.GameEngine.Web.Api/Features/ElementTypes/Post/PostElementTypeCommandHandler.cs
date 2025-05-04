using CF.GameEngine.Web.Api.Features.ElementTypes.Upsert;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Exceptions;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Post;

public class PostElementTypeCommandHandler(IMediator mediator) : IUnitRequestHandler<PostElementTypeCommand, Guid>
{
    public async Task<IUnitResult<Guid>> Handle(PostElementTypeCommand request, CancellationToken cancellationToken)
    {
        if(request.ElementType.Id != Guid.Empty)
        {
            return new UnitResult(new InvalidEntityStateException("ElementType", "ElementType ID must be empty for creation.")).As<Guid>();
        }

        return await mediator.Send(new ElementTypeUpsertCommand(request.ElementType), cancellationToken);
    }
}
