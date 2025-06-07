using System.Collections.Concurrent;

namespace IDFCR.Shared.Abstractions.Roles;

public static class RoleRegistrar
{
    internal readonly static Lazy<ConcurrentBag<IRoleRegistrar>> globalRegistrars = new (() => []);
    private static IEnumerable<IRoleDescriptor> FilterByCategory(IEnumerable<IRoleDescriptor> roles, RoleCategory? category) =>
    category.HasValue ? roles.Where(r => r.Category == category.Value) : roles;

    public static IEnumerable<IRoleRegistrar> GlobalRegistrars => [.. globalRegistrars.Value];

    public static void RegisterGlobal<T>()
     where T : IRoleRegistrar, new()
    {
        if (!GlobalRegistrars.Any(r => r is T))
        {
            globalRegistrars.Value.Add(new T());
        }
    }

    public static IEnumerable<string> List<T>(RoleCategory? category = null, params string[] additionalRoles)
        where T : IRoleRegistrar, new()
    {
        var registrar = new T();

        var roles = FilterByCategory(registrar, category);
        var globalRoles = FilterByCategory(GlobalRegistrars.SelectMany(x => x), category);

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

    public bool TryRegisterRole(string roleName, IRoleDescriptorBuilder roleDescriptor, out string key)
    {
        key = $"{Prefix}{roleName}";
        if (string.IsNullOrWhiteSpace(roleName) || _roles.ContainsKey(key))
        {
            return false;
        }

        roleDescriptor.Key = key;

        return _roles.TryAdd(key, roleDescriptor.Build());
    }

    public IEnumerator<IRoleDescriptor> GetEnumerator() => _roles.Values.GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    public bool TryRegisterRole(string roleName, Action<IRoleDescriptorBuilder> buildRole)
    {
        return TryRegisterRole(roleName, RoleCategory.None, buildRole);
    }

    public bool TryRegisterRole(string roleName, RoleCategory roleCategory, Action<IRoleDescriptorBuilder> buildRole)
    {
        var builder = new RoleDescriptorBuilder(roleCategory);
        buildRole(builder);
        
        return TryRegisterRole(roleName, builder, out _);
    }
}
