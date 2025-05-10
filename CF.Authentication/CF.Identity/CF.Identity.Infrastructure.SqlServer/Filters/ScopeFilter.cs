using CF.Identity.Infrastructure.Features.Scope;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.SqlServer.Filters;

internal class ScopeFilter(IScopeFilter filter) : FilterBase<IScopeFilter, DbScope>(filter), IScopeFilter
{
    protected override IScopeFilter Source => this;
    public IEnumerable<string>? Keys { get; set; }
    public Guid? ClientId { get; set; }
    public string? Key { get; set; }

    public override void Map(IScopeFilter source)
    {
        Key = source.Key;
        Keys = source.Keys;
        ClientId = source.ClientId;
    }
}
