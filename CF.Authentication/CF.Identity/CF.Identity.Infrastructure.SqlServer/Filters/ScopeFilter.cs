using CF.Identity.Infrastructure.Features.Scope;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions;
using LinqKit;

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

    public override ExpressionStarter<DbScope> ApplyFilter(ExpressionStarter<DbScope> query, IScopeFilter filter)
    {
        if(!string.IsNullOrWhiteSpace(filter.Key))
        {
            query = query.And(x => x.Key == filter.Key);
        }

        if (filter.ClientId.HasValue)
        {
            query = query.And(x => x.ClientId == filter.ClientId.Value);
        }

        if (filter.Keys != null && filter.Keys.Any())
        {
            query = query.And(x => filter.Keys.Contains(x.Key));
        }

        return query;
    }
}
