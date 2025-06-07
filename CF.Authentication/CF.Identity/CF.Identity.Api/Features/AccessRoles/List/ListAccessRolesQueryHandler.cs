using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.AccessRoles.List;

public class ListAccessRolesQueryHandler(IAccessRoleRepository accessRoleRepository) : IUnitPagedRequestHandler<ListAccessRolesQuery, AccessRoleDto>
{
    public async Task<IUnitPagedResult<AccessRoleDto>> Handle(ListAccessRolesQuery request, CancellationToken cancellationToken)
    {
        var result = await accessRoleRepository.ListRolesAsync(request, cancellationToken);

        return result.Convert(x => x.Map<AccessRoleDto>());
    }
}