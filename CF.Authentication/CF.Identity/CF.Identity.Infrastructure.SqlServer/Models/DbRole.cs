using CF.Identity.Infrastructure.Features.Roles;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.SqlServer.Models;

public class DbRole : MappableBase<IRole>, IRole
{
    protected override IRole Source => this;

    public Guid ClientId { get; set; }
    public string Key { get; set; } = null!;
    public string? DisplayName { get; set; }
    public Guid Id { get; set; }

    public virtual DbClient Client { get; set; } = null!;
    public virtual ICollection<DbUserRole> UserRoles { get; set; } = [];
    public virtual ICollection<DbRoleScope> Scopes { get; set; } = [];

    public override void Map(IRole source)
    {
        Id = source.Id;
        ClientId = source.ClientId;
        Key = source.Key;
        DisplayName = source.DisplayName;
    }
}
