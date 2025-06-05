using FluentValidation;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Http.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace IDFCR.Shared.Http.Extensions;

public static class ApiResultExtensions
{
    private static int GetStatusCode(IUnitResult result)
    {
        return GetStatusCode(result.Action, result.Exception, result.FailureReason);
    }

    private static int GetStatusCode(this UnitAction action, Exception? exception, FailureReason? failureReason = null)
    {
        if (action != UnitAction.Conflict && exception is ValidationException)
        {
            return StatusCodes.Status400BadRequest;
        }

        if (action != UnitAction.Conflict && failureReason.HasValue)
        {
            return failureReason switch
            {
                FailureReason.NotFound => StatusCodes.Status404NotFound,
                FailureReason.Conflict => StatusCodes.Status409Conflict,
                FailureReason.ValidationError => StatusCodes.Status422UnprocessableEntity,
                FailureReason.Unauthorized => StatusCodes.Status401Unauthorized,
                FailureReason.Forbidden => StatusCodes.Status403Forbidden,
                FailureReason.InternalError => StatusCodes.Status500InternalServerError,
                _ => GetStatusCode(action, exception)
            };
        }

        return action switch
        {
            UnitAction.Add => StatusCodes.Status201Created,
            UnitAction.Update => StatusCodes.Status200OK,
            UnitAction.Get => StatusCodes.Status200OK,
            UnitAction.Delete => StatusCodes.Status204NoContent,
            UnitAction.Pending => StatusCodes.Status202Accepted,
            UnitAction.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        };
    }

    private static void ConfigureHeaders<T>(this IApiResult apiResult, IUnitResult<T> result, string? location = null)
    {
        if (result.Action == UnitAction.Add || result.Action == UnitAction.Update)
        {
            apiResult.AddHeader("Location", $"{location}/{result.Result}");
        }
        else if (result.Action == UnitAction.Pending)
        {
            apiResult.AddHeader("Status-Location", $"{location}/{result.Result}/status");
        }
    }

    private static void ConfigureHeaders<T>(this IApiResult apiResult, IUnitResultCollection<T> result, string? location = null)
    {
        List<string> locationValuesList = [];
        if (result.Action == UnitAction.Add || result.Action == UnitAction.Update || result.Action == UnitAction.Pending)
        {
            foreach (var resultItem in result.Result)
            {
                locationValuesList.Add($"{location}/{resultItem}");
            }
        }

        StringValues locationValues = locationValuesList.ToArray();

        if (result.Action == UnitAction.Add || result.Action == UnitAction.Update)
        {
            apiResult.AddHeader("Location", locationValues);
        }
        else if (result.Action == UnitAction.Pending)
        {
            locationValues = locationValuesList.Select(x => x + "/status").ToArray();
            apiResult.AddHeader("Status-Location", locationValues);
        }
    }

    public static IApiResult ToApiResult(this IUnitResult result)
    {
        var statusCode = GetStatusCode(result);

        ApiResult? apiResult = null;
        if (result.IsSuccess)
        {
            apiResult = new ApiResult(statusCode);
        }
        apiResult ??= new ApiResult(statusCode, result.Exception);
        apiResult.AppendMeta(result.ToDictionary());
        return apiResult;
    }

    public static IApiResult ToApiCollectionResult<T>(this IUnitPagedResult<T> result, string? location = null)
    {
        return ToApiCollectionResult((IUnitResultCollection<T>)result, location);
    }

    public static IApiResult ToApiCollectionResult<T>(this IUnitResultCollection<T> result, string? location = null)
    {
        var statusCode = GetStatusCode(result);
        ApiResult? apiResult = null;

        if (result.Result is not null && result.IsSuccess)
        {
            apiResult = new ApiCollectionResult<T>(result.Result, statusCode);
            ConfigureHeaders(apiResult, result, location);
        }

        apiResult ??= new ApiResult(statusCode, result.Exception);

        apiResult.AppendMeta(result.ToDictionary());

        return apiResult;
    }

    public static IApiResult ToApiResult<T>(this IUnitResult<T> result, string? location = null)
    {
        var statusCode = GetStatusCode(result);

        ApiResult? apiResult = null;

        if (result.Result is not null && result.IsSuccess)
        {
            apiResult = new ApiResult<T>(result.Result, statusCode);

            ConfigureHeaders(apiResult, result, location);
        }

        apiResult ??= new ApiResult(statusCode, result.Exception);

        apiResult.AppendMeta(result.ToDictionary());

        return apiResult;
    }

    public static IApiResult NegotiateResult<T>(this IUnitResultCollection<T> result, IHttpContextAccessor contextAccessor, string? location = null)
    {
        var context = contextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(contextAccessor));
        var acceptHeader = context.Request.Headers.Accept.ToString();
        if (acceptHeader.Contains("application/hal+json"))
        {
            return result.ToHypermediaCollectionResult(location);
        }
        
        return result.ToApiCollectionResult(location);
    }

    public static IApiResult NegotiateResult<T>(this IUnitResult<T> result, IHttpContextAccessor contextAccessor, string? location = null)
    {
        var context = contextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(contextAccessor));

        var acceptHeader = context.Request.Headers.Accept.ToString();

        if (acceptHeader.Contains("application/hal+json"))
        {
            return result.ToHypermediaResult(location);
        }

        return result.ToApiResult(location);
    }

    public static IApiResult NegotiateResult<T>(this IUnitPagedResult<T> result, IHttpContextAccessor contextAccessor, string? location = null)
    {
        return NegotiateResult((IUnitResultCollection<T>)result, contextAccessor, location);
    }

    public static IApiResult ToHypermediaCollectionResult<T>(this IUnitResultCollection<T> result, string? location = null)
    {
        var statusCode = GetStatusCode(result);

        IApiResult? apiResult = null;
        if(result.IsSuccess && result.Result is not null)
        {
            apiResult = new HypermediaApiListResult<T>(result.Result, statusCode);
            apiResult.AppendMeta(result.ToDictionary());

            ConfigureHeaders(apiResult, result, location);
        }

        apiResult ??= new ApiResult(statusCode, result.Exception);
        apiResult.AppendMeta(result.ToDictionary());
        return apiResult;
    }

    public static IApiResult ToHypermediaCollectionResult<T>(this IUnitPagedResult<T> result, string? location = null)
    {
        return ToHypermediaCollectionResult((IUnitResultCollection<T>)result, location);
    }

    public static IApiResult ToHypermediaResult<T>(this IUnitResult<T> result, string? location = null)
    {
        var statusCode = GetStatusCode(result);

        IApiResult? apiResult = null;
        if (result.IsSuccess && result.Result is not null)
        {
            apiResult = new HypermediaApiResult<T>(result.Result, statusCode);
            
            ConfigureHeaders(apiResult, result, location);
        }

        apiResult ??= new ApiResult(statusCode, result.Exception);
        apiResult.AppendMeta(result.ToDictionary());

        return apiResult;
    }

}
