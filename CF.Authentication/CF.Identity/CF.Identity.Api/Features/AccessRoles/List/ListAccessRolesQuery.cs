using CF.Identity.Infrastructure.Features.AccessRoles;
using CF.Identity.Infrastructure.Features;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;
using IDFCR.Shared.Abstractions.Paging;

namespace CF.Identity.Api.Features.AccessRoles.List;

public record ListAccessRolesQuery(Guid? ClientId = null, string? NameContains = null,
    bool NoTracking = true, bool Bypass = false) 
    : PagedQuery, IUnitPagedRequest<AccessRoleDto>, IPagedAccessRoleFilter, IRoleRequirement
{
    public string? SortField { get; init; }
    public string? SortOrder { get; init; }

    IEnumerable<string> IRoleRequirement.Roles => [SystemRoles.GlobalRead, Roles.RoleRead];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
