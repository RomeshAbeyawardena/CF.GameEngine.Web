using CF.Identity.Api.Features.Scopes.Post;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions.Records;

namespace CF.Identity.Api.Endpoints.Roles.Post;

public record PostScopeRequest(string Key, string Name) : MappableBase<IScope>
{
    public string? Client { get; init; }
    public Guid? ClientId { get; init; }
    public string? Description { get; init; }
    protected override IScope Source => new ScopeDto
    {
        ClientId = ClientId,
        Name = Name,
        Description = Description,
        Key = Key
    };

    public override void Map(IScope source)
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
