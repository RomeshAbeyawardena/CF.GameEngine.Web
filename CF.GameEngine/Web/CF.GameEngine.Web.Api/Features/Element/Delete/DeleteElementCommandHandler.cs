using CF.GameEngine.Infrastructure.Features.Elements;
using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.Element.Delete;

public class DeleteElementCommandHandler(IElementRepository elementRepository) : IRequestHandler<DeleteElementCommand, IUnitResult>
{
    public Task<IUnitResult> Handle(DeleteElementCommand request, CancellationToken cancellationToken)
    {
        return elementRepository.DeleteAsync([request.ElementId], cancellationToken);
    }
}