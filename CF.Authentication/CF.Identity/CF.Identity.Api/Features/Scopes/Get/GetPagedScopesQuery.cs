using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions.Paging;
using IDFCR.Shared.Mediatr;
using IRoleRequirement = IDFCR.Shared.Abstractions.IRoleRequirement;
using RoleRequirementType = IDFCR.Shared.Abstractions.RoleRequirementType;

namespace CF.Identity.Api.Features.Scopes.Get;

public record GetPagedScopesQuery
    : MappablePagedQuery<IPagedScopeFilter>, IUnitPagedRequest<ScopeDto>,
      IPagedScopeFilter, 
      IRoleRequirement
{
    protected override IPagedScopeFilter Source => this;
    IEnumerable<string> IRoleRequirement.Roles => [SystemRoles.GlobalRead, ScopeRoles.ScopeRead];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;

    public Guid? ClientId { get; set; }
    public Guid? UserId { get; set; }
    public string? Key { get; set; }
    public bool IncludePrivilegedScoped { get; set; }
    public IEnumerable<string>? Keys { get; set; }
    public bool NoTracking { get; set; }
    public bool Bypass { get; set; }
    public string? SortField { get; set; }
    public string? SortOrder { get; set; }

    public override void Map(IPagedScopeFilter source)
    {
        ClientId = source.ClientId;
        UserId = source.UserId;
        Key = source.Key;
        Keys = source.Keys;
        IncludePrivilegedScoped = source.IncludePrivilegedScoped;
        NoTracking = source.NoTracking;
        SortField = source.SortField;
        SortOrder = source.SortOrder;
    }
}
