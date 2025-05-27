using CF.Identity.Infrastructure.Features.Scope;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Filters;
using LinqKit;

namespace CF.Identity.Infrastructure.SqlServer.Filters;

internal class ScopeFilter(IScopeFilter filter) : MappableFilterBase<IScopeFilter, DbScope>(filter), IScopeFilter
{
    protected override IScopeFilter Source => this;
    public bool IncludePrivilegedScoped { get; set; }
    public IEnumerable<string>? Keys { get; set; }
    public Guid? UserId { get; set; }
    public Guid? ClientId { get; set; }
    public string? Key { get; set; }

    public override void Map(IScopeFilter source)
    {
        Key = source.Key;
        Keys = source.Keys;
        UserId = source.UserId;
        ClientId = source.ClientId;
    }

    public override ExpressionStarter<DbScope> ApplyFilter(ExpressionStarter<DbScope> query, IScopeFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Key) && filter.Keys?.Any() == true)
        {
            throw new InvalidOperationException("Cannot filter by both Key and Keys simultaneously.");
        }

        if (!string.IsNullOrWhiteSpace(filter.Key))
        {
            query = query.And(x => x.Key == filter.Key);
        }

        if (filter.ClientId.HasValue)
        {
            //gets all filters for the client including scopes that are global
            query = query.And(x => !x.ClientId.HasValue || x.ClientId == filter.ClientId.Value);
        }

        if (filter.UserId.HasValue)
        {
            query = query.And(x => x.UserScopes.Any(x => x.UserId == filter.UserId.Value));
        }

        List<string> scopeKeys;
        if (filter.Keys != null && (scopeKeys = [.. filter.Keys.Take(50)]).Count > 0)
        {
            query = query.And(x => scopeKeys.Contains(x.Key));
        }

        if (!filter.IncludePrivilegedScoped)
        {
            query = query.And(x => !x.IsPrivileged);
        }

        return query;
    }
}
