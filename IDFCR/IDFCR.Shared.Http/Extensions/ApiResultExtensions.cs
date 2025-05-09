using FluentValidation;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Http.Results;
using Microsoft.AspNetCore.Http;

namespace IDFCR.Shared.Http.Extensions;

public static class ApiResultExtensions
{
    private static int GetStatusCode(this UnitAction action, Exception? exception)
    {
        if(exception is ValidationException)
        {
            return StatusCodes.Status400BadRequest;
        }

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
        var statusCode = GetStatusCode(result.Action, result.Exception);

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
        var statusCode = GetStatusCode(result.Action, result.Exception);

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

    public static IApiResult NegotiateResult<T>(this IUnitResult<T> result, IHttpContextAccessor contextAccessor, string? location = null)
    {
        var context = contextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(contextAccessor));

        var acceptHeader = context.Request.Headers.Accept.ToString();

        if (acceptHeader.Contains("application/hal+json"))
        {
            return result.ToHypermediaResult(location);
        }

        //Defaults to JSON
        if (string.IsNullOrWhiteSpace(location))
        {
            return result.ToApiResult();
        }

        return result.ToApiResult(location);
    }

    internal static IApiResult ToHypermediaResultSingleton<T>(this IUnitResult<T> result, string? location = null)
    {
        var statusCode = GetStatusCode(result.Action, result.Exception);
        var singleResult = new HypermediaApiResult<T>(result.Result, statusCode);
        singleResult.AppendMeta(result.ToDictionary());

        if (location is not null && result.Action == UnitAction.Add)
        {
            singleResult.AddHeader("Location", $"{location}/{result.Result}");
        }

        return singleResult;
    }

    public static IApiResult ToHypermediaResult<T>(this IUnitResult<T> result, string? location = null)
    {
        var statusCode = GetStatusCode(result.Action, result.Exception);

        if (result.IsSuccess && result.Result is not null)
        {
            // Handle collection
            if (result.Result.GetType().IsCollection(out var t))
            {
                var genericHyperMediaType = typeof(HypermediaApiListResult<>).MakeGenericType(t ?? throw new NullReferenceException("No generic type found"));
                var instance = Activator.CreateInstance(genericHyperMediaType, [result.Result, statusCode]) ?? throw new InvalidOperationException("Failed to create instance of HypermediaApiListResult");
                var listResult = (ApiResult)instance;
                listResult.AppendMeta(result.ToDictionary());
                return listResult;
            }

            return ToHypermediaResultSingleton(result, location);
        }

        // Failure fallback
        var errorResult = new ApiResult(statusCode, result.Exception);
        errorResult.AppendMeta(result.ToDictionary());
        return errorResult;
    }

}
