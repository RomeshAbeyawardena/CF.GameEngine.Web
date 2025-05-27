using CF.Identity.Api.Features.AccessRoles.Upsert;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.Identity.Api.Features.AccessRoles.Post;

public class PostAccessRoleCommandHandler(IMediator mediator) : IUnitRequestHandler<PostAccessRoleCommand, Guid>
{
    public async Task<IUnitResult<Guid>> Handle(PostAccessRoleCommand request, CancellationToken cancellationToken)
    {
        return await mediator.Send(new UpsertAccessRoleCommand(request.AccessRole, request.Bypass), cancellationToken);
    }
}