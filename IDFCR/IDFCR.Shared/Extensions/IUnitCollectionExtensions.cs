using IDFCR.Shared.Abstractions.Results;

namespace IDFCR.Shared.Extensions;

public static class UnitCollectionExtensions
{
    public static IUnitResult<T> GetOne<T>(this IUnitResultCollection<T> collection,
        Func<T, bool>? predicate = null, int? elementIndex = null, object? id = null,
        Func<IEnumerable<T>, IOrderedEnumerable<T>>? orderedTranform = null) where T : class
    {
        var result = GetOneOrDefault(collection, predicate, elementIndex, orderedTranform);

        if (result is null)
        {
            return UnitResult.NotFound<T>(id ?? "predicate");
        }

        return UnitResult.FromResult(result, collection.Action, collection.IsSuccess, collection.Exception);
    }

    public static T? GetOneOrDefault<T>(this IUnitResultCollection<T> collection, 
        Func<T, bool>? predicate = null, int? elementIndex = null,
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

            return elementIndex.HasValue 
                ? results.ElementAtOrDefault(elementIndex.Value) 
                : results.FirstOrDefault(predicate);
        }

        return elementIndex.HasValue 
            ? results.ElementAtOrDefault(elementIndex.Value) 
            : results.FirstOrDefault();
    }
}
