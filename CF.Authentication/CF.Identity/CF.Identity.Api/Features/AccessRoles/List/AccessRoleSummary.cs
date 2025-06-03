using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Api.Features.AccessRoles.List;

public class AccessRoleSummary : MappableBase<IAccessRole>, IAccessRoleSummary
{
    protected override IAccessRole Source => new AccessRoleDto
    {
        ClientId = ClientId,
        Key = Key,
        DisplayName = DisplayName,
        Id = Id
    };

    public Guid ClientId { get; set; }
    public string Key { get; set; } = null!;
    public string? DisplayName { get; set; }
    public Guid Id { get; set; }

    public override void Map(IAccessRole source)
    {
        ClientId = source.ClientId;
        Key = source.Key;
        DisplayName = source.DisplayName;
        Id = source.Id;
    }
}
