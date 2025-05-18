using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.SqlServer.Models;

public class DbUserScope : IIdentifer
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ScopeId { get; set; }

    public virtual DbUser User { get; set; } = null!;
    public virtual DbScope Scope { get; set; } = null!;
}
