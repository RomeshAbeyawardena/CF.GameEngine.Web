namespace CF.Identity.Infrastructure.Features.Users;

public interface IUserCasingImpression
{
    string EmailAddressCI { get; }
    string UsernameCI { get; }
    string? PreferredUsernameCI { get; }
}
