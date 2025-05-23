using CF.Identity.Api.Features.Clients;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions.Records;

namespace CF.Identity.Api.Endpoints.Clients.Post;

public record PostClientRequest(string Reference, string Name, string? Secret, DateTimeOffset ValidFrom)
    : MappableBase<IClient>
{
    protected override IClient Source => new EditableClientDto {
        Reference = Reference,
        Name = Name,
        SecretHash = Secret,
        ValidFrom = ValidFrom,
        DisplayName = DisplayName,
        ValidTo = ValidTo,
    };
    public string? DisplayName { get; init; }
    public DateTimeOffset? ValidTo { get; init; }

    public override void Map(IClient source)
    {
        throw new NotImplementedException();
    }
}
