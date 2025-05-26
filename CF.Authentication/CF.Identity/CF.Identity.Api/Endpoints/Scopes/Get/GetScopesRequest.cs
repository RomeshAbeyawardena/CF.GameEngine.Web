using CF.Identity.Api.Features.Scopes.Get;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions.Records;

namespace CF.Identity.Api.Endpoints.Scopes.Get;

public record GetScopesRequest(string? Key = null, IEnumerable<string>? Keys = null) : MappableBase<IPagedScopeFilter>
{
    protected override IPagedScopeFilter Source => new GetPagedScopesQuery
    {
        Key = Key,
        Keys = Keys,
        UserId = UserId,
        ClientId = ClientId,
        SortField = SortField,
        SortOrder = SortOrder
    };

    public Guid? UserId { get; init; }
    public Guid? ClientId { get; set; }
    public string? SortField { get; init; }
    public string? SortOrder { get; init; }

    public override void Map(IPagedScopeFilter source)
    {
        ClientId = source.ClientId;
    }

    public GetPagedScopesQuery ToQuery()
    {
        return Map<GetPagedScopesQuery>();
    }
}
