using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.AccessRoles.Get;

public record GetAccessRolesQuery(Guid? ClientId = null, string? Name = null, string? NameContains = null,
    bool NoTracking = true, bool Bypass = false) : IUnitRequestCollection<AccessRoleDto>, IAccessRoleFilter, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [SystemRoles.GlobalRead, Roles.RoleRead];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}

public class GetAccessRolesQueryHandler(IAccessRoleRepository accessRoleRepository) : IUnitRequestCollectionHandler<GetAccessRolesQuery, AccessRoleDto>
{
    public async Task<IUnitResultCollection<AccessRoleDto>> Handle(GetAccessRolesQuery request, CancellationToken cancellationToken)
    {
        var results = await accessRoleRepository.GetAccessRolesAsync(request, cancellationToken);

        return results.Convert(x => x.Map<AccessRoleDto>());
    }
}