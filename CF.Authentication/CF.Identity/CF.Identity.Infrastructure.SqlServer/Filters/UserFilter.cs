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
        return query;
    }

    public override void Map(IUserFilter source)
    {
        throw new NotImplementedException();
    }
}
