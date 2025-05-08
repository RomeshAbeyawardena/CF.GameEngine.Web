using System.Reflection;

namespace IDFCR.Shared.Http.Extensions;

public static class TypeExtensions
{
    public static bool IsCollection(this Type value, out Type? genericType)
    {
        genericType = null;
        if (value.IsArray)
        {
            return true;
        }

        if (value.IsGenericType)
        {
            genericType = value.GetGenericArguments().FirstOrDefault();
            var interfaces = value.GetInterfaces();
            return interfaces.Any(i =>
                i.IsGenericType &&
                (
                    i.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                    i.GetGenericTypeDefinition() == typeof(ICollection<>) ||
                    i.GetGenericTypeDefinition() == typeof(IList<>)
                )
            );
        }

        return false;
    }

}
