namespace IDFCR.Shared.Abstractions;

public static class RoleRegistrar
{
    public static IEnumerable<string> List<T>(RoleCategory? category = null)
        where T : IRoleRegistrar, new()
    {
        var registrar = new T();

        return category.HasValue 
            ? registrar.Where(r => r.Category == category).Select(role => role.Key)
            : registrar.Select(role => role.Key);
    }

    public static string FlattenedRoles<T>(RoleCategory? category = null)
        where T : IRoleRegistrar, new()
    {
        return TransformRoles<T>(x => string.Join(',', x), category);
    }

    public static string TransformRoles<T>(Func<IEnumerable<string>, string> transformer, RoleCategory? category = null)
        where T : IRoleRegistrar, new()
    {
        return transformer(List<T>(category));
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
