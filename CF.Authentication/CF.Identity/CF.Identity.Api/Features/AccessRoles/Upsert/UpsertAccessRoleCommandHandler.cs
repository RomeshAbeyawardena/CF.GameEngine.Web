using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.AccessRoles.Upsert;

public class UpsertAccessRoleCommandHandler(IAccessRoleRepository accessRoleRepository) : IUnitRequestHandler<UpsertAccessRoleCommand, Guid>
{
    public async Task<IUnitResult<Guid>> Handle(UpsertAccessRoleCommand request, CancellationToken cancellationToken)
    {
        return await accessRoleRepository.UpsertAsync(request.AccessRole.Map<Infrastructure.Features.AccessRoles.AccessRoleDto>(), cancellationToken);
    }
}