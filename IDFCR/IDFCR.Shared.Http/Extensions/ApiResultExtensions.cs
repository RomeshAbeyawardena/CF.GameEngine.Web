using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Http.Results;
using Microsoft.AspNetCore.Http;

namespace IDFCR.Shared.Http.Extensions;

public static class ApiResultExtensions
{
    public static IApiResult ToApiResult(this IUnitResult result)
    {
        var statusCode = result.Action switch
        {
            UnitAction.Add => StatusCodes.Status201Created,
            UnitAction.Update => StatusCodes.Status200OK,
            UnitAction.Delete => StatusCodes.Status204NoContent,
            _ => StatusCodes.Status500InternalServerError,
        };
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
        var statusCode = result.Action switch
        {
            UnitAction.Add => StatusCodes.Status201Created,
            UnitAction.Update => StatusCodes.Status200OK,
            UnitAction.Delete => StatusCodes.Status204NoContent,
            _ => StatusCodes.Status500InternalServerError,
        };

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
}
