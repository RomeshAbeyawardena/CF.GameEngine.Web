using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.SqlServer.Models;

//this has no interface because it is not used in the API, only in the database
public class DbRoleScope : IIdentifer
{
    public Guid Id { get; set; }
    public Guid AccessRoleId { get; set; }
    public Guid ScopeId { get; set; }
    public virtual DbAccessRole Role { get; set; } = null!;
    public virtual DbScope Scope { get; set; } = null!;
}
