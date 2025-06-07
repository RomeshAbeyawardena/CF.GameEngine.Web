using CF.Identity.Api.Features.User.Upsert;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.EntityFramework;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.Identity.Api.Features.User.Post;

public class PostUserCommandHandler(IMediator mediator, ITransactionalUnitOfWork transactionalUnitOfWork) : IUnitRequestHandler<PostUserCommand, Guid>
{
    public async Task<IUnitResult<Guid>> Handle(PostUserCommand request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpsertUserCommand(request.User), cancellationToken);

        if (result.IsSuccess)
        {
            await transactionalUnitOfWork.SaveChangesAsync(cancellationToken);
        }
        return result;
    }
}
