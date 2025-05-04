using CF.GameEngine.Infrastructure.Features.Elements;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.Element.Delete;

public class DeleteElementCommandHandler(IElementRepository elementRepository) : IUnitRequestHandler<DeleteElementCommand>
{
    public Task<IUnitResult> Handle(DeleteElementCommand request, CancellationToken cancellationToken)
    {
        return elementRepository.DeleteAsync([request.ElementId], cancellationToken);
    }
}