using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.SqlServer.Filters;

internal class ClientFilter(IClientFilter filter) : FilterBase<IClientFilter, DbClient>(filter), IClientFilter
{
    protected override IClientFilter Source => this;
    public string? Key { get; set; }
    public override void Map(IClientFilter source)
    {
        Key = source.Key;
    }
}
