using LinqKit;
using IDFCR.Shared.Abstractions.Filters;

namespace IDFCR.Shared.Extensions;

public static class ExpressionStarterExtensions
{
    public static ExpressionStarter<T> FilterValidity<T>(this ExpressionStarter<T> query, IValidityFilter filter)
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
}
