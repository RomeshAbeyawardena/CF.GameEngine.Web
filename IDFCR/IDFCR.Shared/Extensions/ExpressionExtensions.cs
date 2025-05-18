using LinqKit;
using IDFCR.Shared.Abstractions;
using System.Linq.Expressions;

namespace IDFCR.Shared.Extensions;

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> FilterValidity<T>(this ExpressionStarter<T> query, IValidityFilter filter)
        where T : IValidity
    {

        if (filter.ValidFrom.HasValue)
        {
            query = query.And(x => x.ValidFrom <= filter.ValidFrom.Value);
        }

        if (filter.ValidTo.HasValue)
        {
            query = query.And(x => !x.ValidTo.HasValue || x.ValidTo >= filter.ValidTo.Value);
        }

        return query;
    }

    /// <summary>
    /// Builds an expression: x => x.Property == null || x.Property.Contains(value)
    /// </summary>
    public static Expression<Func<T, bool>> OrNullContains<T>(
        Expression<Func<T, string?>> propertySelector,
        string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return x => true; // No filter

        var parameter = propertySelector.Parameters[0];
        var member = propertySelector.Body;

        var nullCheck = Expression.Equal(member, Expression.Constant(null, typeof(string)));
        var containsCall = Expression.Call(
            member,
            typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!,
            Expression.Constant(value, typeof(string))
        );

        var body = Expression.OrElse(nullCheck, containsCall);

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}
