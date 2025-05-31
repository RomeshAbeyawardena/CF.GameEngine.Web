using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.SqlServer.Models;

public class DbCommonName : MappableBase<ICommonName>, ICommonName
{
    protected override ICommonName Source => this;

    public Guid Id { get; set; }
    public string Value { get; set; } = null!;
    public string ValueCI { get; set; } = null!;
    public string ValueHmac { get; set; } = null!;
    
    public string RowVersion { get; set; } = null!;
    public string MetaData { get; set; } = null!;
    public bool IsAnonymisedMarker { get; set; }

    public override void Map(ICommonName source)
    {
        Id = source.Id;
        Value = source.Value;
        RowVersion = source.RowVersion;
        MetaData = source.MetaData;
        IsAnonymisedMarker = source.IsAnonymisedMarker;
    }
}
