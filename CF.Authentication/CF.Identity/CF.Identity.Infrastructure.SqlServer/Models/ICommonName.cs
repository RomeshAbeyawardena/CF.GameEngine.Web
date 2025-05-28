using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.SqlServer.Models;

public interface ICommonName : IMappable<ICommonName>, IIdentifer
{
    string Value { get; }
    string ValueNormalised { get; }
    string RowVersion { get; }
    string MetaData { get; }
}

