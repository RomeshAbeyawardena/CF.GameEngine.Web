using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Paging;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;

using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;

namespace CF.Identity.Api.Features.AccessRoles.List;

public record ListAccessRolesQuery
    : MappablePagedQuery<IPagedAccessRoleFilter>, IUnitPagedRequest<AccessRoleDto>, IPagedAccessRoleFilter 
{
    IEnumerable<string> IRoleRequirement.Roles => RoleRegistrar.List<Roles>(RoleCategory.Read);

    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;

    protected override IPagedAccessRoleFilter Source => this;

    public Guid? ClientId { get; set; }
    public string? NameContains { get; set; }
    public string? Name { get; set; } = null!;
    public bool NoTracking { get; set; } = true;
    public bool Bypass { get; set; }

    public string? SortField { get; init; }
    public string? SortOrder { get; init; }

    public override void Map(IPagedAccessRoleFilter source)
    {
        ClientId = source.ClientId;
        NameContains = source.NameContains;
        Name = source.Name;
        NoTracking = source.NoTracking;
        PageIndex = source.PageIndex;
        PageSize = source.PageSize;
    }
}
