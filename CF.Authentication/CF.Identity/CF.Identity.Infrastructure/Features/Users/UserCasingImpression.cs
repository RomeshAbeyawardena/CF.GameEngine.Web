using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Users;

public class UserCasingImpression(IUser user) : IUserCasingImpression
{

    public string EmailAddressCI => CasingImpression.Calculate(user.EmailAddress);
    public string UsernameCI => CasingImpression.Calculate(user.Username);
    public string? PreferredUsernameCI => string.IsNullOrWhiteSpace(user.PreferredUsername) ? null : CasingImpression.Calculate(user.PreferredUsername);
}
