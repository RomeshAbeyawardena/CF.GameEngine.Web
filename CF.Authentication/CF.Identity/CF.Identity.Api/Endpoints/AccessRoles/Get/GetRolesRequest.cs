using CF.Identity.Api.Features.AccessRoles.List;
using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions.Filters;
using IDFCR.Shared.Abstractions.Paging;

namespace CF.Identity.Api.Endpoints.AccessRoles.Get;

public record GetRolesRequest : MappablePagedQuery<IPagedAccessRoleFilter>, IPagedAccessRoleFilter
{
    bool IFilter.NoTracking => true;

    protected override IPagedAccessRoleFilter Source => new ListAccessRolesQuery()
    {
        ClientId = ClientId,
        Name = Name,
        NameContains = NameContains,
        NoTracking = true,
        SortField = SortField,
        SortOrder = SortOrder
    };

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
