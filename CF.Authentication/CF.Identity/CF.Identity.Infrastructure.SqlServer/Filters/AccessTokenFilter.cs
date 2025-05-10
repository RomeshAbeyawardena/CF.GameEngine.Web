using CF.Identity.Infrastructure.Features.AccessToken;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Filters;
using LinqKit;

namespace CF.Identity.Infrastructure.SqlServer.Filters;

internal class AccessTokenFilter(IAccessTokenFilter filter) : FilterBase<IAccessTokenFilter, DbAccessToken>(filter), IAccessTokenFilter
{
    protected override IAccessTokenFilter Source => this;
    public string? ReferenceToken { get; set;}
    public Guid? ClientId { get; set; }
    public string? Type { get; set; }
    public DateTimeOffset? ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }

    public override void Map(IAccessTokenFilter source)
    {
        ReferenceToken = source.ReferenceToken;
        ClientId = source.ClientId;
        Type = source.Type;
        ValidFrom = source.ValidFrom;
        ValidTo = source.ValidTo;
    }

    public override ExpressionStarter<DbAccessToken> ApplyFilter(ExpressionStarter<DbAccessToken> query, IAccessTokenFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.ReferenceToken))
        {
            query = query.And(x => x.ReferenceToken == filter.ReferenceToken);
        }

        if (filter.ClientId.HasValue)
        {
            query = query.And(x => x.ClientId == filter.ClientId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Type))
        {
            query = query.And(x => x.Type == filter.Type);
        }

        if (filter.ValidFrom.HasValue)
        {
            query = query.And(x => x.ValidFrom >= filter.ValidFrom.Value);
        }

        if (filter.ValidTo.HasValue)
        {
            query = query.And(x => x.ValidTo <= filter.ValidTo.Value);
        }

        return query;
    }
}
