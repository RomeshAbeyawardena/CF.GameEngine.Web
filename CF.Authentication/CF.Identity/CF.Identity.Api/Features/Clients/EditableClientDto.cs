using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Api.Features.Clients;

public class EditableClientDto : MappableBase<IClient>, IEditableClient, IClient
{
    protected override IClient Source => this;
    public string? SecretHash { get; set; }
    public DateTimeOffset? SuspendedTimestampUtc { get; set; }
    public string Reference { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? DisplayName { get; set; }
    public bool IsSystem { get; set; }
    public Guid Id { get; set; }
    public DateTimeOffset ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }

    public override void Map(IClient source)
    {
        SecretHash = source.SecretHash;
        SuspendedTimestampUtc = source.SuspendedTimestampUtc;
        Reference = source.Reference;
        Name = source.Name;
        DisplayName = source.DisplayName;
        IsSystem = source.IsSystem;
        Id = source.Id;
        ValidFrom = source.ValidFrom;
        ValidTo = source.ValidTo;
    }
}
