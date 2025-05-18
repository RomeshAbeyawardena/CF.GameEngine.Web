using Microsoft.Extensions.Configuration;

namespace CF.Identity.MigrationUtility;

public class UserInfo
{
    public UserInfo(IConfiguration configuration)
    {
        var userSetting = configuration.GetSection("Seed:User");

        userSetting.Bind(this);
    }

    public string Password { get; set; } = null!;
    public string EmailAddress { get; set; } = null!;
    public string PreferredUsername { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string PrimaryTelephoneNumber { get; set; } = null!;
}
