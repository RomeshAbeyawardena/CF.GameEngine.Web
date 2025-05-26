using CF.Identity.Api.Features.Scopes.Get;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions.Records;

namespace CF.Identity.Api.Endpoints.Scopes.Get;

public record GetScopesRequest(string? Key = null, IEnumerable<string>? Keys = null) : MappableBase<IScopeFilter>
{
    protected override IScopeFilter Source => new GetPagedScopesQuery
    {
        Key = Key,
        Keys = Keys,
        UserId = UserId,
        ClientId = ClientId
    };

    public Guid? UserId { get; init; }
    public Guid? ClientId { get; set; }

    public override void Map(IScopeFilter source)
    {
        ClientId = source.ClientId;
    }

    public GetPagedScopesQuery ToQuery()
    {
        return Map<GetPagedScopesQuery>();
    }
}
