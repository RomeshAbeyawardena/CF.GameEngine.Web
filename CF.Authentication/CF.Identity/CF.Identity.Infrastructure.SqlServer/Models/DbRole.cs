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

    public override void Map(IRole source)
    {
        Id = source.Id;
        ClientId = source.ClientId;
        Key = source.Key;
        DisplayName = source.DisplayName;
    }
}

//this has no interface because it is not used in the API, only in the database
public class DbRoleScope : IIdentifer
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public Guid ScopeId { get; set; }
    public virtual DbRole Role { get; set; } = null!;
    public virtual DbScope Scope { get; set; } = null!;
}

//this has no interface because it is not used in the API, only in the database
public class DbUserRole : IIdentifer
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    public ICollection<DbRoleScope> Scopes { get; set; } = [];

    public virtual DbUser User { get; set; } = null!;
    public virtual DbRole Role { get; set; } = null!;
}