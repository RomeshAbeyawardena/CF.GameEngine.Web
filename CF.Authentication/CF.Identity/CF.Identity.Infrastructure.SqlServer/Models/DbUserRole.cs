using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.SqlServer.Models;

//this has no interface because it is not used in the API, only in the database
public class DbUserRole : IIdentifer
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid AccessRoleId { get; set; }

    public ICollection<DbRoleScope> Scopes { get; set; } = [];
    public virtual ICollection<DbUserRole> UserRoles { get; set; } = [];

    public virtual DbUser User { get; set; } = null!;
    public virtual DbAccessRole AccessRole { get; set; } = null!;
}