using CF.Identity.Api.Features.Scopes.Post;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions.Records;

namespace CF.Identity.Api.Endpoints.Scopes.Post;

public record PostScopeRequest(string Key, string Name) : MappableBase<IScope>
{
    public string? Client { get; init; }
    public Guid? ClientId { get; init; }
    public string? Description { get; init; }

    protected override IScope Source => new EditableScopeDto
    {
        Client = Client,
        ClientId = ClientId,
        Description = Description,
        Key = Key,
        Name = Name
    };

    public override void Map(IScope source)
    {
        throw new NotSupportedException();
    }

    public EditableScopeDto ToEditable()
    {
        var editableScope = Map<EditableScopeDto>();
        editableScope.Client = Client;

        return editableScope;
    }
}
