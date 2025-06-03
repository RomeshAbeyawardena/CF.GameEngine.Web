using CF.Identity.Infrastructure.Features.AccessRoles;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Filters;
using LinqKit;

namespace CF.Identity.Infrastructure.SqlServer.Filters;

internal class AccessRoleFilter(IAccessRoleFilter filter) : MappableFilterBase<IAccessRoleFilter, DbAccessRole>(filter), IAccessRoleFilter
{
    public Guid? ClientId { get; set; }
    public string? Name { get; set; }
    public string? NameContains { get; set; }

    protected override IAccessRoleFilter Source => this;

    public override ExpressionStarter<DbAccessRole> ApplyFilter(ExpressionStarter<DbAccessRole> query, IAccessRoleFilter filter)
    {
        if (ClientId.HasValue)
        {
            query = query.And(x => x.ClientId == ClientId);
        }

        if (!string.IsNullOrWhiteSpace(Name))
        {
            query = query.And(x => x.Key == Name);
        }

        if (!string.IsNullOrWhiteSpace(NameContains))
        {
            query = query.And(x => x.Key.Contains(NameContains));
        }

        return query;
    }

    public override void Map(IAccessRoleFilter source)
    {
        ClientId = source.ClientId;
        Name = source.Name;
        NameContains = source.NameContains;
    }
}
