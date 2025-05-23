using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Api.Features.Clients;

public class EditableClientDto : MappableBase<IClient>, IEditableClient, IClient
{
    protected override IClient Source => this;
    public string? SecretHash { get; set; }
    public DateTimeOffset? SuspendedTimestampUtc { get; set; }
    public string Reference { get; set; }
    public string Name { get; set; }
    public string? DisplayName { get; set; }
    public bool IsSystem { get; set; }
    public Guid Id { get; set; }
    public DateTimeOffset ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }

    public override void Map(IClient source)
    {
        throw new NotImplementedException();
    }
}
