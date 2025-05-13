using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Filters;
using LinqKit;

namespace CF.Identity.Infrastructure.SqlServer.Filters;

public class UserFilter(IUserFilter filter) : FilterBase<IUserFilter, DbUser>(filter), IUserFilter
{
    protected override IUserFilter Source => this;
    public Guid? ClientId { get; set; }
    public string? NameContains { get; set; }

    public override ExpressionStarter<DbUser> ApplyFilter(ExpressionStarter<DbUser> query, IUserFilter filter)
    {
        if (filter.ClientId.HasValue)
        {
            query = query.And(x => x.ClientId == filter.ClientId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.NameContains))
        {
            query = query.And(x => x.Firstname.Contains(filter.NameContains) 
                || x.LastName.Contains(filter.NameContains)
                || x.Username.Contains(filter.NameContains)
                || (x.MiddleName == null || x.MiddleName.Contains(filter.NameContains)));
        }

        return query;
    }

    public override void Map(IUserFilter source)
    {
        ClientId = source.ClientId;
        NameContains = source.NameContains;
    }
}
