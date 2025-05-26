namespace CF.Identity.Infrastructure.Features.Users;

public interface IUserHmac
{
    string EmailAddressHmac { get; set; }
    string UsernameHmac { get; set; }
    string PreferredUsernameHmac { get; set; }
    string PrimaryTelephoneNumberHmac { get; set; }
}