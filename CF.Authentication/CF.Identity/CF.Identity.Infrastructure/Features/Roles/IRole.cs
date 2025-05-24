using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Roles;

public interface IRole : IMappable<IRole>, IIdentifer
{
    //Non-optional, these will belong to a given a client and no-one else
    Guid ClientId { get; }
    string Key { get; }
    string? DisplayName { get; }
}

public class RoleDto : MappableBase<IRole>, IRole
{
    protected override IRole Source => this;
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public override void Map(IRole source)
    {
        Id = source.Id;
        ClientId = source.ClientId;
        Key = source.Key;
        DisplayName = source.DisplayName;
    }
}