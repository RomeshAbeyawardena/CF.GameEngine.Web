using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.AccessRoles;

public class AccessRoleDto : MappableBase<IAccessRole>, IAccessRole
{
    protected override IAccessRole Source => this;
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public override void Map(IAccessRole source)
    {
        Id = source.Id;
        ClientId = source.ClientId;
        Key = source.Key;
        DisplayName = source.DisplayName;
        Description = source.Description;
    }
}