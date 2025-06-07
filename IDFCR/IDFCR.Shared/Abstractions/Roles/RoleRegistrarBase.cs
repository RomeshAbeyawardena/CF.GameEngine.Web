namespace IDFCR.Shared.Abstractions.Roles;

public static class RoleRegistrar
{
    private readonly static List<IRoleRegistrar> globalRegistrars = [];
    public static IEnumerable<IRoleRegistrar> GlobalRegistrars => globalRegistrars;

    public static void SetGlobalRegistrar<T>()
        where T : IRoleRegistrar, new()
    {
        globalRegistrars.Add(new T());
    }

    public static IEnumerable<string> List<T>(RoleCategory? category = null, params string[] additionalRoles)
        where T : IRoleRegistrar, new()
    {
        var registrar = new T();

        var roles = category.HasValue ? registrar.Where(r => r.Category == category)
            : registrar;

        var globalRoles = GlobalRegistrars.SelectMany(x => x)?.Where(r => r.Category == category) ?? [];

        if (globalRoles.Any())
        {
            roles = roles.Union(globalRoles);
        }

        return [.. roles.Select(role => role.Key).Union(additionalRoles)];
    }

    public static string FlattenedRoles<T>(RoleCategory? category = null, params string[] additionalRoles)
        where T : IRoleRegistrar, new()
    {
        return TransformRoles<T>(x => string.Join(',', x), category, additionalRoles);
    }

    public static string TransformRoles<T>(Func<IEnumerable<string>, string> transformer, RoleCategory? category = null, params string[] additionalRoles)
        where T : IRoleRegistrar, new()
    {
        return transformer(List<T>(category, additionalRoles));
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
        return TryRegisterRole(roleName, RoleCategory.None, buildRole);
    }

    public bool TryRegisterRole(string roleName, RoleCategory roleCategory, Action<IRoleDescriptorBuilder> buildRole)
    {
        var builder = new RoleDescriptorBuilder(roleName, roleCategory);
        buildRole(builder);

        return TryRegisterRole(roleName, builder.Build(), out _);
    }
}
