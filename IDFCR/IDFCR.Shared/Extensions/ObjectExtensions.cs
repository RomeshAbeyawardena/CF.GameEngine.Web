using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace IDFCR.Shared.Extensions;

public static class ObjectExtensions
{
    private static readonly ConcurrentDictionary<(Type Source, Type Target), Action<object, object>> _applyCache = new();

    public static void Apply(this object target, object source)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);

        var sourceType = source.GetType();
        var targetType = target.GetType();

        var action = _applyCache.GetOrAdd((sourceType, targetType), CreateApplyAction);
        action(source, target);
    }

    private static Action<object, object> CreateApplyAction((Type Source, Type Target) types)
    {
        var (sourceType, targetType) = types;

        var sourceParam = Expression.Parameter(typeof(object), "source");
        var targetParam = Expression.Parameter(typeof(object), "target");

        var sourceCast = Expression.Convert(sourceParam, sourceType);
        var targetCast = Expression.Convert(targetParam, targetType);

        var blockExpressions = new List<Expression>();

        foreach (var targetProp in targetType.GetProperties().Where(p => p.CanWrite))
        {
            var sourceProp = sourceType.GetProperty(targetProp.Name);
            if (sourceProp != null && sourceProp.CanRead && sourceProp.PropertyType == targetProp.PropertyType)
            {
                var sourceValue = Expression.Property(sourceCast, sourceProp);
                var targetValue = Expression.Property(targetCast, targetProp);

                // if (!Equals(targetValue, sourceValue))
                var notEqual = Expression.Not(
                    Expression.Call(
                        typeof(object),
                        nameof(object.Equals),
                        Type.EmptyTypes,
                        Expression.Convert(targetValue, typeof(object)),
                        Expression.Convert(sourceValue, typeof(object))
                    )
                );

                // target.Property = source.Property;
                var assign = Expression.Assign(targetValue, sourceValue);

                // if (!Equals(...)) { target.Property = source.Property; }
                var ifNotEqualAssign = Expression.IfThen(notEqual, assign);

                blockExpressions.Add(ifNotEqualAssign);
            }
        }

        var body = Expression.Block(blockExpressions);

        var lambda = Expression.Lambda<Action<object, object>>(body, sourceParam, targetParam);
        return lambda.Compile();
    }
}
