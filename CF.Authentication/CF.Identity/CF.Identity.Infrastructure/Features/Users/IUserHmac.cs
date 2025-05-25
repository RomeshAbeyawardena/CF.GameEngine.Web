using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Users;

public interface IUserCasingImpression
{
    string EmailAddressCI { get; }
    string UsernameCI { get; }
    string? PreferredUsernameCI { get; }
}

public class UserCasingImpression(IUser user) : IUserCasingImpression
{

    public string EmailAddressCI => CasingImpression.Calculate(user.EmailAddress);
    public string UsernameCI => CasingImpression.Calculate(user.Username);
    public string? PreferredUsernameCI => string.IsNullOrWhiteSpace(user.PreferredUsername) ? null : CasingImpression.Calculate(user.PreferredUsername);
}

public interface IUserHmac
{
    string EmailAddressHmac { get; set; }
    string UsernameHmac { get; set; }
    string PreferredUsernameHmac { get; set; }
    string PrimaryTelephoneNumberHmac { get; set; }
}

public record UserHmac : IUserHmac
{
    public required string EmailAddressHmac {get; set; } 
    public required string UsernameHmac { get; set; }
    public required string PreferredUsernameHmac { get; set; }
    public required string PrimaryTelephoneNumberHmac { get; set; }
}