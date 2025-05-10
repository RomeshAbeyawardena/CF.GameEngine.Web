using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Filters;
using LinqKit;

namespace CF.Identity.Infrastructure.SqlServer.Filters;

internal class ClientFilter(IClientFilter filter) : FilterBase<IClientFilter, DbClient>(filter), IClientFilter
{
    protected override IClientFilter Source => this;
    public string? Key { get; set; }
    public override void Map(IClientFilter source)
    {
        Key = source.Key;
    }

    public override ExpressionStarter<DbClient> ApplyFilter(ExpressionStarter<DbClient> query, IClientFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Key))
        {
            query = query.And(x => x.Reference == filter.Key || x.Name == filter.Key);
        }

        return query;
    }
}
