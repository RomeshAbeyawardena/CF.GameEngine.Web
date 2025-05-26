using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Users;

public class UserCasingImpression(IUser user) : IUserCasingImpression
{
    public string EmailAddressCI { get; } = CasingImpression.Calculate(user.EmailAddress);
    public string UsernameCI { get; } = CasingImpression.Calculate(user.Username);
    public string? PreferredUsernameCI { get; } = string.IsNullOrWhiteSpace(user.PreferredUsername) ? null : CasingImpression.Calculate(user.PreferredUsername);
}
