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
            query = query.And(a => a.ValidFrom >= filter.ValidFrom);
        }

        if (filter.ValidTo.HasValue)
        {
            query = query.And(a => a.ValidTo <= filter.ValidTo);
        }

        return query;
    }
}
