using CF.Identity.Api.Features.User.Upsert;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.Identity.Api.Features.User.Post;

public class PostUserCommandHandler(IMediator mediator) : IUnitRequestHandler<PostUserCommand, Guid>
{
    public async Task<IUnitResult<Guid>> Handle(PostUserCommand request, CancellationToken cancellationToken)
    { 
        return await mediator.Send(new UpsertUserCommand(request.User), cancellationToken);
    }
}
