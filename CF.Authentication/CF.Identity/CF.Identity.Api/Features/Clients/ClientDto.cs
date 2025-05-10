using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Api.Features.Clients;

public class ClientDto : MappableBase<IClient>, IEditableClient, IClient
{
    protected override IClient Source => this;
    public DateTimeOffset? SuspendedTimestampUtc { get; set; }
    public string Reference { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? DisplayName { get; set; }
    public Guid Id { get; set; }
    public DateTimeOffset ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }
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
