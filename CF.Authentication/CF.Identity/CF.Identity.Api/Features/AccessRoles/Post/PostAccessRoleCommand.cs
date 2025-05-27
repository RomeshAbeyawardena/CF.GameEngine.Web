using CF.Identity.Api.Features.AccessRoles.Upsert;
using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.Identity.Api.Features.AccessRoles.Post;

public record PostAccessRoleCommand(EditableAccessRoleDto AccessRole, bool Bypass = false) : IUnitRequest<Guid>, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [SystemRoles.GlobalWrite, Roles.RoleWrite];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}

public class PostAccessRoleCommandHandler(IMediator mediator) : IUnitRequestHandler<PostAccessRoleCommand, Guid>
{
    public async Task<IUnitResult<Guid>> Handle(PostAccessRoleCommand request, CancellationToken cancellationToken)
    {
        return await mediator.Send(new UpsertAccessRoleCommand(request.AccessRole, request.Bypass), cancellationToken);
    }
}