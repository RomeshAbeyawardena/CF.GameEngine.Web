using IDFCR.Shared.Abstractions.Results;

namespace IDFCR.Shared.Extensions;

public static class UnitResultExtensions
{
    public static IUnitResult<TDestination> Convert<T, TDestination>(this IUnitResult<T> unitResult, Func<T, TDestination> converter)
    {
        if(unitResult.Result is null)
        {
            return new UnitResult(unitResult.Exception, unitResult.Action, unitResult.IsSuccess).As<TDestination>();
        }

        return new UnitResult<TDestination>(converter(unitResult.Result), unitResult.Action, unitResult.IsSuccess, unitResult.Exception);
    }
}
