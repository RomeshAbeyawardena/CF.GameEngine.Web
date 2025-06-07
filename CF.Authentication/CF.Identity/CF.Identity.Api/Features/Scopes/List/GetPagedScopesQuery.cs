using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Paging;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Scopes.List;

[RoleRequirement(nameof(GetRoles))]
public record GetPagedScopesQuery
    : MappablePagedQuery<IPagedScopeFilter>, IUnitPagedRequest<ScopeDto>,
      IPagedScopeFilter
{
    protected override IPagedScopeFilter Source => this;

    public static readonly Func<IEnumerable<string>> GetRoles = () => RoleRegistrar.List<ScopeRoles>(RoleCategory.Read);
    
    public Guid? ClientId { get; set; }
    public Guid? UserId { get; set; }
    public string? Key { get; set; }
    public bool IncludePrivilegedScoped { get; set; }
    public IEnumerable<string>? Keys { get; set; }
    public bool NoTracking { get; set; }
    public bool Bypass { get; set; } = false;
    public string? SortField { get; set; }
    public string? SortOrder { get; set; }

    public override void Map(IPagedScopeFilter source)
    {
        ClientId = source.ClientId;
        UserId = source.UserId;
        Key = source.Key;
        Keys = source.Keys;
        PageSize = source.PageSize;
        PageIndex = source.PageIndex;
        IncludePrivilegedScoped = source.IncludePrivilegedScoped;
        NoTracking = source.NoTracking;
        SortField = source.SortField;
        SortOrder = source.SortOrder;
    }
}
