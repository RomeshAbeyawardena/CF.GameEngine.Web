using CF.Identity.Infrastructure.Features.AccessRoles;
using CF.Identity.Infrastructure.Features;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;
using IDFCR.Shared.Abstractions.Paging;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;

namespace CF.Identity.Api.Features.AccessRoles.List;

public record ListAccessRolesQuery
    : MappablePagedQuery<IPagedAccessRoleFilter>, IUnitPagedRequest<AccessRoleDto>, IPagedAccessRoleFilter, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [SystemRoles.GlobalRead, Roles.RoleRead];
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

public class ListAccessRolesQueryHandler(IAccessRoleRepository accessRoleRepository) : IUnitPagedRequestHandler<ListAccessRolesQuery, AccessRoleDto>
{
    public async Task<IUnitPagedResult<AccessRoleDto>> Handle(ListAccessRolesQuery request, CancellationToken cancellationToken)
    {
        var result = await accessRoleRepository.ListRolesAsync(request, cancellationToken);

        return result.Convert(x => x.Map<AccessRoleDto>());
    }
}