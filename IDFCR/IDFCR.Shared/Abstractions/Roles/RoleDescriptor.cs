namespace IDFCR.Shared.Abstractions.Roles;

public static class RoleDescriptor
{
    public static string ConcatenateRoles(char? separator = null, params string[] roles)
    {
        if (!separator.HasValue)
        {
            separator = ' ';
        }

        return string.Join(separator.Value, roles);
    }
}
