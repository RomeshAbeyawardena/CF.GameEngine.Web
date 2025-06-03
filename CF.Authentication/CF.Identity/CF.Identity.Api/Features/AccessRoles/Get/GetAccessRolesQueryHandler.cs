using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.AccessRoles.Get;

public class GetAccessRolesQueryHandler(IAccessRoleRepository accessRoleRepository) : IUnitRequestCollectionHandler<GetAccessRolesQuery, AccessRoleDto>
{
    public async Task<IUnitResultCollection<AccessRoleDto>> Handle(GetAccessRolesQuery request, CancellationToken cancellationToken)
    {
        var results = await accessRoleRepository.GetAccessRolesAsync(request, cancellationToken);

        return results.Convert(x => x.Map<AccessRoleDto>());
    }
}