using IDFCR.Shared.Abstractions.Results;

namespace IDFCR.Shared.Extensions;

public static class UnitCollectionExtensions
{
    public static IUnitResultCollection<T> ToUnitCollection<T>(this IEnumerable<T> items, UnitAction action = UnitAction.Get, bool isSuccess = true)
    {
        return UnitResultCollection.FromResult(items, action, isSuccess);
    }

    public static List<T> AsList<T>(this IUnitResultCollection<T> collection) where T : class
    {
        if (!collection.HasValue)
        {
            return [];
        }

        return collection.Result.ToList();
    }

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
        if (!collection.HasValue)
        {
            return null;
        }

        var results = collection.Result;

        if (predicate is not null)
        {
            results = results.Where(predicate);
        }

        if (orderedTranform is not null)
        {
            results = orderedTranform(results);
        }

        return elementIndex.HasValue 
            ? results.ElementAtOrDefault(elementIndex.Value) 
            : results.FirstOrDefault();
    }
}
