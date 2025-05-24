using CF.Identity.Api.Features.Scopes.Post;
using CF.Identity.Infrastructure.Features.Roles;
using IDFCR.Shared.Abstractions.Records;

namespace CF.Identity.Api.Endpoints.Roles.Post;

public record PostRoleRequest(string Key, string Name) : MappableBase<IRole>
{
    public string? Client { get; init; }
    public Guid? ClientId { get; init; }
    public string? Description { get; init; }
    protected override IRole Source => new RoleDto
    {
        ClientId = ClientId.GetValueOrDefault(),
        Key = Key,
        DisplayName = Name,
    };

    public override void Map(IRole source)
    {
        throw new NotSupportedException();
    }

    public EditableScopeDto ConvertToEditable()
    {
        var scope = this.Map<EditableScopeDto>();
        scope.Client = Client;
        return scope;
    }
}
