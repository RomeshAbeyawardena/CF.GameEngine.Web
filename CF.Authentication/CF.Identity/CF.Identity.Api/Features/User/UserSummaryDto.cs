using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Api.Features.User;

public class UserSummaryDto : MappableBase<IUser>, IUserSummary
{
    protected override IUser Source => new UserDto
    {
        IsSystem = IsSystem,
        ClientId = ClientId,
        PreferredUsername = PreferredUsername,
        Id = Id
    };

    public string Display => PreferredUsername ?? $"User-{Id.ToString("N")[..6]}";
    public bool IsSystem { get; set; }
    public Guid ClientId { get; set; }
    public string? PreferredUsername { get; set; }
    public Guid Id { get; set; }

    public override void Map(IUser source)
    {
        IsSystem = source.IsSystem;
        ClientId = source.ClientId;
        PreferredUsername = source.PreferredUsername;
        Id = source.Id;
    }
}
