
namespace IDFCR.Shared.Abstractions;

public abstract class RoleRegistrarBase : IRoleRegistrar
{
    protected readonly Dictionary<string, IRoleDescriptor> _roles = [];

    public string? Prefix { get; set; }

    public bool TryRegisterRole(string roleName, bool isPrivileged = false)
    {
        if (string.IsNullOrWhiteSpace(roleName) || _roles.ContainsKey(roleName))
        {
            return false;
        }
        var key = $"{Prefix}{roleName}";
        _roles.TryAdd(key, new DefaultRoleDescriptor(key, isPrivileged) { Prefix = Prefix});
        return true;
    }

    public IEnumerator<IRoleDescriptor> GetEnumerator() => _roles.Values.GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<string> RegisterRoles(params string[] roleNames)
    {
        var failedRoles = new List<string>();
        foreach (var roleName in roleNames)
        {
            if (!TryRegisterRole(roleName))
            {
                failedRoles.Add(roleName);
            }
        }

        return failedRoles;
    }
}
