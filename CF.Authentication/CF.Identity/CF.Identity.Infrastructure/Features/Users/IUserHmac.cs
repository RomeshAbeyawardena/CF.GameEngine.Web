namespace CF.Identity.Infrastructure.Features.Users;

public interface IUserHmac
{
    string EmailAddressHmac { get; set; }
    string UsernameHmac { get; set; }
    string PreferredUsernameHmac { get; set; }
}

public record UserHmac : IUserHmac
{
    public required string EmailAddressHmac {get; set; } 
    public required string UsernameHmac { get; set; }
    public required string PreferredUsernameHmac { get; set; }
}
