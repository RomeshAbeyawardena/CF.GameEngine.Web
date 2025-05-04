using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions.Records;

namespace CF.Identity.Api.Features.Client;

public record ClientResponse : MappableBase<IClient>, IClientSummary
{
    protected override IClient Source => new ClientDto
    {
        Reference = Reference,
        Name = Name,
        DisplayName = DisplayName,
        Id = Id
    };

    public string Reference { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? DisplayName { get; set; }
    public Guid Id { get; set; }

    public override void Map(IClient source)
    {
        Reference = source.Reference;
        Name = source.Name;
        DisplayName = source.DisplayName;
        Id = source.Id;
    }
}
