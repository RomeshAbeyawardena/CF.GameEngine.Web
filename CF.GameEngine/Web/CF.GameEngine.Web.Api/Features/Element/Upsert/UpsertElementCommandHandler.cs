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
        var result = await elementRepository.UpsertAsync(request.Element.Map<Infrastructure.Features.Elements.ElementDto>(), cancellationToken);

        await elementRepository.SaveChangesAsync(cancellationToken);

        return result;
    }
}