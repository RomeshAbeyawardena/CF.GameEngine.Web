using CF.Identity.Api.Features.Scopes.List;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions.Records;

namespace CF.Identity.Api.Endpoints.Scopes.Get;

public record GetScopesRequest : MappableBase<IPagedScopeFilter>
{
    protected override IPagedScopeFilter Source => new GetPagedScopesQuery
    {
        Key = Key,
        Keys = Keys,
        UserId = UserId,
        ClientId = ClientId,
        PageIndex = PageIndex,
        PageSize = PageSize,
        SortField = SortField,
        SortOrder = SortOrder,
        Bypass = false,
    };

    public string? Key { get; set; }
    public IEnumerable<string>? Keys { get; set; }

    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
    public Guid? UserId { get; set; }
    public Guid? ClientId { get; set; }
    public string? SortField { get; set; }
    public string? SortOrder { get; set; }

    public override void Map(IPagedScopeFilter source)
    {
        ClientId = source.ClientId;
    }

    public GetPagedScopesQuery ToQuery()
    {
        return Map<GetPagedScopesQuery>();
    }
}
