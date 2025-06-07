using CF.Identity.Infrastructure.Features.AccessToken;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Filters;
using IDFCR.Shared.Extensions;
using LinqKit;

namespace CF.Identity.Infrastructure.SqlServer.Filters;

internal class AccessTokenFilter(IAccessTokenFilter filter) : MappableFilterBase<IAccessTokenFilter, DbAccessToken>(filter), IAccessTokenFilter
{
    protected override IAccessTokenFilter Source => this;
    public string? ReferenceToken { get; set; }
    public Guid? ClientId { get; set; }
    public string? Type { get; set; }
    public DateTimeOffset? ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }
    public bool ShowAll { get; set; }
    public Guid? UserId { get; set; }

    public override void Map(IAccessTokenFilter source)
    {
        ReferenceToken = source.ReferenceToken;
        ClientId = source.ClientId;
        UserId = source.UserId;
        Type = source.Type;
        ValidFrom = source.ValidFrom;
        ValidTo = source.ValidTo;
        ShowAll = source.ShowAll;
    }

    public override ExpressionStarter<DbAccessToken> ApplyFilter(ExpressionStarter<DbAccessToken> query, IAccessTokenFilter filter)
    {
        if (!ShowAll)
        {
            query = query.And(x => !x.SuspendedTimestampUtc.HasValue);
        }

        if (!string.IsNullOrWhiteSpace(filter.ReferenceToken))
        {
            query = query.And(x => x.ReferenceToken == filter.ReferenceToken);
        }

        if (filter.ClientId.HasValue)
        {
            query = query.And(x => x.ClientId == filter.ClientId);
        }

        if (filter.UserId.HasValue)
        {
            query = query.And(x => x.UserId == filter.UserId);
        }

        if (!string.IsNullOrWhiteSpace(filter.Type))
        {
            query = query.And(x => x.Type == filter.Type);
        }

        return query.FilterValidity(filter);
    }
}
