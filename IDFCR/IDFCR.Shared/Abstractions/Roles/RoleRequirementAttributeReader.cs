using System.Reflection;

namespace IDFCR.Shared.Abstractions.Roles;

public static class RoleRequirementAttributeReader
{
    public static IRoleRequirement? GetRoleRequirement<T>()
    {
        var type = typeof(T);
        var attribute = type.GetCustomAttribute<RoleRequirementAttribute>();

        if(attribute is null)
        {
            return null;
        }

        var field = type.GetField(attribute.ActionName, BindingFlags.Static | BindingFlags.Public) 
            ?? throw new NullReferenceException($"No public static field named '{attribute.ActionName}' found on type '{type.Name}'.");

        if (field.FieldType != typeof(Func<IEnumerable<string>>))
            throw new InvalidOperationException($"Field '{attribute.ActionName}' must be of type Func<IEnumerable<string>>.");

        var instance = Activator.CreateInstance(typeof(RoleRequirement), 
            [field.GetValue(null), attribute.RoleRequirementType]);

        return instance as RoleRequirement ?? throw new InvalidCastException();
    }
}