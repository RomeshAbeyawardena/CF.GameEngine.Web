using IDFCR.Shared.Extensions;
using IDFCR.Shared.Http.Results;
using System.Linq.Expressions;
using System.Reflection;

namespace IDFCR.Shared.Http.Extensions;

public static class ObjectExtensions
{
    private static readonly Lazy<Dictionary<Type, Func<object, Dictionary<string, object?>>>> _cache = new(() => []);

    public static IEntryWrapper<T> AsEntryWrapper<T>(this T entry)
    {
        return new EntryWrapper<T>(entry);
    }

    private static Func<object, Dictionary<string, object?>> BuildConverter(Type type)
    {
        var input = Expression.Parameter(typeof(object), "input");
        var typedInput = Expression.Variable(type, "typed");

        var resultVar = Expression.Variable(typeof(Dictionary<string, object?>), "result");

        var assignTyped = Expression.Assign(typedInput, Expression.Convert(input, type));
        var assignResult = Expression.Assign(resultVar, Expression.New(typeof(Dictionary<string, object?>)));

        var blockExpressions = new List<Expression>
        {
            assignTyped,
            assignResult
        };

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!prop.CanRead)
            {
                continue;
            }

            var getProp = Expression.Property(typedInput, prop);
            var key = Expression.Constant(prop.Name.ToCamelCasePreservingAcronyms());
            var value = Expression.Convert(getProp, typeof(object));

            var addCall = Expression.Call(
                resultVar,
                typeof(Dictionary<string, object?>).GetMethod("Add")!,
                key,
                value
            );

            blockExpressions.Add(addCall);
        }

        blockExpressions.Add(resultVar); // final return

        var body = Expression.Block([typedInput, resultVar], blockExpressions);
        var lambda = Expression.Lambda<Func<object, Dictionary<string, object?>>>(body, input);

        return lambda.Compile();
    }
    
    public static IDictionary<string, object?> AsDictionary(this object value)
    {
        if (value is null)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
        }

        var type = value.GetType();

        if (type.IsGenericType)
        {
            return new Dictionary<string, object?>();
        }

        if (!_cache.Value.TryGetValue(type, out var converter))
        {
            converter = BuildConverter(type);
            _cache.Value[type] = converter;
        }

        return new Dictionary<string, object?>(converter(value), new CaseInsensitiveComparer<string>());
    }
}
