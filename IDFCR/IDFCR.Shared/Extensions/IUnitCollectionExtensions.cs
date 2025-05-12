using IDFCR.Shared.Abstractions.Results;

namespace IDFCR.Shared.Extensions;

public static class UnitCollectionExtensions
{
    public static T? GetOneOrDefault<T>(this IUnitResultCollection<T> collection, 
        Func<T, bool>? predicate = null,
        Func<IEnumerable<T>, IOrderedEnumerable<T>>? orderedTranform = null) where T : class
    {
        if (!collection.IsSuccess || collection.Result is null)
        {
            return null;
        }

        var results = collection.Result;

        if (predicate is not null)
        {
            if(orderedTranform is not null)
            {
                results = orderedTranform(results);
            }

            return results.FirstOrDefault(predicate);
        }

        return results.FirstOrDefault();
    }
}
