using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Api.Features.Scopes;

public class ScopeDto : MappableBase<IScope>, IScope
{
    protected override IScope Source => this;
    public string Key { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid Id { get; set; }
    public Guid? ClientId { get; set; }
    public bool IsPrivileged { get; set; }

    public override void Map(IScope source)
    {
        Key = source.Key;
        Name = source.Name;
        Description = source.Description;
        Id = source.Id;
        ClientId = source.ClientId;
    }
}
