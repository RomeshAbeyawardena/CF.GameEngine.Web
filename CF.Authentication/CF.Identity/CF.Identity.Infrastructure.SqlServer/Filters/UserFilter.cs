using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Filters;
using IDFCR.Shared.Extensions;
using LinqKit;

namespace CF.Identity.Infrastructure.SqlServer.Filters;

public class UserFilter(IUserFilter filter) : FilterBase<IUserFilter, DbUser>(filter), IUserFilter
{
    protected override IUserFilter Source => this;
    public Guid? ClientId { get; set; }
    public string? NameContains { get; set; }
    public bool? IsSystem { get; set; }

    public override ExpressionStarter<DbUser> ApplyFilter(ExpressionStarter<DbUser> query, IUserFilter filter)
    {
        if (filter.ClientId.HasValue)
        {
            query = query.And(x => x.ClientId == filter.ClientId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.NameContains))
        {
            var nameMatch = PredicateBuilder.New<DbUser>(true);
            nameMatch = nameMatch.Or(x => x.FirstCommonName.ValueNormalised.Contains(filter.NameContains))
                .Or(x => x.LastCommonName.ValueNormalised.Contains(filter.NameContains))
                .Or(x => x.Username.Contains(filter.NameContains))
                .Or(ExpressionExtensions.OrNullContains<DbUser>(u => u.MiddleCommonName.ValueNormalised, NameContains));

            query = query.And(nameMatch);
        }

        if (filter.IsSystem.HasValue)
        {
            query = query.And(x => x.IsSystem == filter.IsSystem);
        }

        return query;
    }

    public override void Map(IUserFilter source)
    {
        ClientId = source.ClientId;
        NameContains = source.NameContains;
    }
}
