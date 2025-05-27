using CF.Identity.Api.Features.AccessRoles;
using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions.Records;

namespace CF.Identity.Api.Endpoints.AccessRoles.Post;

public record PostRoleRequest(string Key, string Name) : MappableBase<IAccessRole>
{
    public string? Client { get; init; }
    public Guid? ClientId { get; init; }
    public string? Description { get; init; }
    protected override IAccessRole Source => new EditableAccessRoleDto
    {
        ClientId = ClientId.GetValueOrDefault(),
        Key = Key,
        DisplayName = Name,
        Description = Description
    };

    public override void Map(IAccessRole source)
    {
        throw new NotSupportedException();
    }

    public EditableAccessRoleDto ConvertToEditable()
    {
        var accessRole = this.Map<EditableAccessRoleDto>();
        accessRole.Client = Client;
        return accessRole;
    }
}
