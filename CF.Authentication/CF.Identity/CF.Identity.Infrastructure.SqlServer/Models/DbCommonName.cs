using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.SqlServer.Models;

public class DbCommonName : IIdentifer
{
    public Guid Id { get; set; }
    public string Value { get; set; } = null!;
    public string ValueNormalised { get; set; } = null!;
    public string RowVersion { get; set; } = null!;
    public string MetaData { get; set; } = null!;
}
