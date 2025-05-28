using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.SqlServer.Models;

internal class CommonNameDto : MappableBase<ICommonName>, ICommonName
{
    protected override ICommonName Source => this;

    public Guid Id { get; set; }
    public string Value { get; set; } = null!;
    public string ValueNormalised { get; set; } = null!;
    public string RowVersion { get; set; } = null!;
    public string MetaData { get; set; } = null!;

    public override void Map(ICommonName source)
    {
        Id = source.Id;
        Value = source.Value;
        ValueNormalised = source.ValueNormalised;
        RowVersion = source.RowVersion;
        MetaData = source.MetaData;
    }
}

