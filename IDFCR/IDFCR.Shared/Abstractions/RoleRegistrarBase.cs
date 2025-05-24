using System.Collections.Concurrent;

namespace IDFCR.Shared.Abstractions;

public abstract class RoleRegistrarBase : IRoleRegistrar
{
    protected readonly ConcurrentBag<string> _roles = [];

    public string? Prefix { get; set; }

    public bool TryRegisterRole(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName) || _roles.Contains(roleName))
        {
            return false;
        }

        _roles.Add($"{Prefix}{roleName}");
        return true;
    }
    public IEnumerator<string> GetEnumerator() => _roles.GetEnumerator();
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
