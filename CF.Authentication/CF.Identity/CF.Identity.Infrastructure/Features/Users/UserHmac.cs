namespace CF.Identity.Infrastructure.Features.Users;

public record UserHmac : IUserHmac
{
    public required string EmailAddressHmac {get; set; } 
    public required string UsernameHmac { get; set; }
    public required string PreferredUsernameHmac { get; set; }
    public required string PrimaryTelephoneNumberHmac { get; set; }
}