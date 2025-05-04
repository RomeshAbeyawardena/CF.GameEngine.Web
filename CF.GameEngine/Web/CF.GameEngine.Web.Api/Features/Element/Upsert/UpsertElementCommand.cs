using CF.GameEngine.Infrastructure.Features.Elements;
using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.Element.Upsert;

public record UpsertElementCommand(ElementDto Element) : IRequest<IUnitResult<Guid>>;

public class UpsertElementCommandHandler(IElementRepository elementRepository) : IRequestHandler<UpsertElementCommand, IUnitResult<Guid>>
{
    public async Task<IUnitResult<Guid>> Handle(UpsertElementCommand request, CancellationToken cancellationToken)
    {
        var result = await elementRepository.UpsertAsync(request.Element.Map<Infrastructure.Features.Elements.ElementDto>(), cancellationToken);

        await elementRepository.SaveChangesAsync(cancellationToken);

        return result;
    }
}