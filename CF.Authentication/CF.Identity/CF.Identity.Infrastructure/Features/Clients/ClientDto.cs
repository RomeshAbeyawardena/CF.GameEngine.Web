using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Clients;

public class ClientDto : MappableBase<IClient>, IClient
{
    protected override IClient Source => this;

    public string Reference { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? DisplayName { get; set; }
    public Guid Id { get; set; }
    public DateTimeOffset ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }
    public DateTimeOffset? SuspendedTimestampUtc { get; set; }
    public string? SecretHash { get; }

    public override void Map(IClient source)
    {
        Reference = source.Reference;
        Name = source.Name;
        DisplayName = source.DisplayName;
        Id = source.Id;
        ValidFrom = source.ValidFrom;
        ValidTo = source.ValidTo;
        SuspendedTimestampUtc = source.SuspendedTimestampUtc;
    }
}
