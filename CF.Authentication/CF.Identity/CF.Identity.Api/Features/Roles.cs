namespace CF.Identity.Api.Features;

public static partial class Roles
{
    public static string ConcatenateRoles(params string[] roles)
    {
        return string.Join(',', roles);
    }
    public const string GlobalRead = "api:read";
    public const string GlobalWrite = "api:write";
}
