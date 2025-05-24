using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Api.Features.Scopes.Post;

public class EditableScopeDto : MappableBase<IScope>, IEditableScope, IScope
{
    protected override IScope Source => this;
    public string Key { get; set; } = null!;
    public bool IsPrivileged { get; set; }
    public string? Client { get; set; }
    public Guid? ClientId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid Id { get; set; }

    public override void Map(IScope source)
    {
        Key = source.Key;
        IsPrivileged = source.IsPrivileged;
        ClientId = source.ClientId;
        Name = source.Name;
        Description = source.Description;
        Id = source.Id;
    }
}
