using CF.GameEngine.Web.Api.Features.Element.Upsert;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.Element.Post;

public class PostElementCommandHandler(IMediator mediator) : IUnitRequestHandler<PostElementCommand, Guid>
{
    public Task<IUnitResult<Guid>> Handle(PostElementCommand request, CancellationToken cancellationToken)
    {
        return mediator.Send(new UpsertElementCommand(request.Element), cancellationToken);
    }
}
