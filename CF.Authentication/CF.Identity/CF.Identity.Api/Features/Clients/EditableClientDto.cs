using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Api.Features.Clients;

public class EditableClientDto : MappableBase<IClient>, IEditableClient
{
    protected override IClient Source => throw new NotImplementedException();
    public string? SecretHash { get; }
    public DateTimeOffset? SuspendedTimestampUtc { get; }
    public string Reference { get; }
    public string Name { get; }
    public string? DisplayName { get; }
    public bool IsSystem { get; }
    public Guid Id { get; }
    public DateTimeOffset ValidFrom { get; }
    public DateTimeOffset? ValidTo { get; }

    public override void Map(IClient source)
    {
        throw new NotImplementedException();
    }
}
