using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.SqlServer.Models;

public class DbScope : MappableBase<IScope>, IScope
{
    protected override IScope Source => this;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid Id { get; set; }
    public string Key { get; set; } = null!;
    public Guid? ClientId { get; set; }
    public bool IsPrivileged { get; set; }

    public virtual DbClient? Client { get; set; }
    public virtual ICollection<DbUserScope> UserScopes { get; set; } = [];
    

    public override void Map(IScope source)
    {
        Name = source.Name;
        Description = source.Description;
        Id = source.Id;
        Key = source.Key;
        ClientId = source.ClientId;
    }
}
