using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.AccessRoles.Find;

public class FindAccessRoleQueryHandler(IAccessRoleRepository accessRoleRepository) : IUnitRequestHandler<FindAccessRoleQuery, AccessRoleDetail>
{
    public async Task<IUnitResult<AccessRoleDetail>> Handle(FindAccessRoleQuery request, CancellationToken cancellationToken)
    {
        var result = await accessRoleRepository.FindAccessTokenAsync(request.AccessRoleId, cancellationToken);

        return result.Convert(x => x.Map<AccessRoleDetail>());
    }
}