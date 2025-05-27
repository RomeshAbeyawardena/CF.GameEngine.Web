namespace IDFCR.Shared.Abstractions;

public static class RoleRegistrar
{
    public static IEnumerable<string> List<T>()
        where T : IRoleRegistrar, new()
    {
        var registrar = new T();
        return registrar.Select(role => role.Key);
    }
}

public abstract class RoleRegistrarBase : IRoleRegistrar
{
    protected readonly Dictionary<string, IRoleDescriptor> _roles = [];
    public string? Prefix { get; set; }

    public bool TryRegisterRole(string roleName, IRoleDescriptor roleDescriptor, out string key)
    {
        key = $"{Prefix}{roleName}";
        if (string.IsNullOrWhiteSpace(roleName) || _roles.ContainsKey(key))
        {
            return false;
        }
        
        return _roles.TryAdd(key, roleDescriptor);
    }

    public IEnumerator<IRoleDescriptor> GetEnumerator() => _roles.Values.GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    public bool TryRegisterRole(string roleName, Action<IRoleDescriptorBuilder> buildRole)
    {
        var builder = new RoleDescriptorBuilder(roleName);
        buildRole(builder);

        return TryRegisterRole(roleName, builder.Build(), out _);
    }
}
