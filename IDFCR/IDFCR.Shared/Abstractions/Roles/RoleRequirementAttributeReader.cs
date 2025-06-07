using System.Reflection;

namespace IDFCR.Shared.Abstractions.Roles;

public static class RoleRequirementAttributeReader
{
    internal static RoleRequirement? GetRoleRequirement(Type type)
    {
        var attribute = type.GetCustomAttribute<RoleRequirementAttribute>();

        if(attribute is null)
        {
            return null;
        }

        type.GetField(attribute.ActionName, BindingFlags.Public | BindingFlags.Static);

        var instance = Activator.CreateInstance(typeof(RoleRequirement), [attribute.RoleRequirementType]);

        return instance as RoleRequirement ?? throw new InvalidCastException();
    }
}