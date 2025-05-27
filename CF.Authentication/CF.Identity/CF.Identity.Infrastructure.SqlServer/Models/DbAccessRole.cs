using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.SqlServer.Models;

public class DbAccessRole : MappableBase<IAccessRole>, IAccessRole
{
    protected override IAccessRole Source => this;
    public Guid ClientId { get; set; }
    public string Key { get; set; } = null!;
    public string? DisplayName { get; set; }
    public Guid Id { get; set; }

    public virtual DbClient Client { get; set; } = null!;
    public virtual ICollection<DbUserRole> UserRoles { get; set; } = [];
    public virtual ICollection<DbRoleScope> Scopes { get; set; } = [];

    public override void Map(IAccessRole source)
    {
        throw new NotImplementedException();
    }
}
