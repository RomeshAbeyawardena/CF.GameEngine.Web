using CF.Identity.Api.Features.AccessRoles.List;
using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions.Records;

namespace CF.Identity.Api.Endpoints.AccessRoles.Get;

public record GetRolesRequest : MappableBase<IPagedAccessRoleFilter>
{
    protected override IPagedAccessRoleFilter Source => new ListAccessRolesQuery()
    {
        ClientId = ClientId,
        Name = Name,
        NameContains = NameContains,
        NoTracking = true,
        PageSize = PageSize,
        PageIndex = PageIndex,
        SortField = SortField,
        SortOrder = SortOrder
    };

    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
    public Guid? ClientId { get; set; }
    public string? Name { get; set; } = null!;
    public string? NameContains { get; set; }
    public string? SortField { get; set; }
    public string? SortOrder { get; set; }

    public ListAccessRolesQuery ToQuery()
    {
        var result = this.Map<ListAccessRolesQuery>();

        return result;
    }

    public override void Map(IPagedAccessRoleFilter source)
    {
        throw new NotSupportedException();
    }
}
