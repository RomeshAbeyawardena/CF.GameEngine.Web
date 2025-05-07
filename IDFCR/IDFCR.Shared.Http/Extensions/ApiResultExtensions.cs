using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Http.Results;
using Microsoft.AspNetCore.Http;

namespace IDFCR.Shared.Http.Extensions;

public static class ApiResultExtensions
{
    private static int GetStatusCode(this UnitAction action)
    {
        return action switch
        {
            UnitAction.Add => StatusCodes.Status201Created,
            UnitAction.Update => StatusCodes.Status200OK,
            UnitAction.Get => StatusCodes.Status200OK,
            UnitAction.Delete => StatusCodes.Status204NoContent,
            _ => StatusCodes.Status500InternalServerError,
        };
    }

    public static IApiResult ToApiResult(this IUnitResult result)
    {
        var statusCode = GetStatusCode(result.Action);

        ApiResult? apiResult = null;
        if (result.IsSuccess)
        {
            apiResult = new ApiResult(statusCode);
        }
        apiResult ??= new ApiResult(statusCode, result.Exception);
        apiResult.AppendMeta(result.ToDictionary());
        return apiResult;
    }

    public static IApiResult ToApiResult<T>(this IUnitResult<T> result, string location)
    {
        var statusCode = GetStatusCode(result.Action);

        ApiResult? apiResult = null;

        if (result.Result is not null && result.IsSuccess)
        {
            apiResult = new ApiResult<T>(result.Result, statusCode);

            if (result.Action == UnitAction.Add)
            {
                apiResult.AddHeader("Location", $"{location}/{result.Result}");
            }
        }

        apiResult ??= new ApiResult(statusCode, result.Exception);

        apiResult.AppendMeta(result.ToDictionary());

        return apiResult;
    }

    public static IApiResult ToHypermediaResult<T>(this IUnitResult<T> result, string? location = null)
    {
        var statusCode = GetStatusCode(result.Action);

        if (result.IsSuccess && result.Result is not null)
        {
            // Handle collection
            if (result.Result is IEnumerable<T> enumerable && typeof(T) != typeof(string))
            {
                var listResult = new HypermediaApiListResult<T>(enumerable, statusCode);
                listResult.AppendMeta(result.ToDictionary());
                return listResult;
            }

            // Handle single
            var singleResult = new HypermediaApiResult<T>(result.Result, statusCode);
            singleResult.AppendMeta(result.ToDictionary());

            if (location is not null && result.Action == UnitAction.Add)
            {
                singleResult.AddHeader("Location", $"{location}/{result.Result}");
            }

            return singleResult;
        }

        // Failure fallback
        var errorResult = new ApiResult(statusCode, result.Exception);
        errorResult.AppendMeta(result.ToDictionary());
        return errorResult;
    }

}
