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

    public static IApiResult ToApiCollectionResult<T>(this IUnitPagedResult<T> result, string location)
    {
        return ToApiCollectionResult((IUnitResultCollection<T>)result, location);
    }

    public static IApiResult ToApiCollectionResult<T>(this IUnitResultCollection<T> result, string location)
    {
        var statusCode = GetStatusCode(result);
        ApiResult? apiResult = null;

        if (result.Result is not null && result.IsSuccess)
        {
            apiResult = new ApiCollectionResult<T>(result.Result, statusCode);

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
                apiResult.AddHeader("Status-Location",  locationValues);
            }
        }

        apiResult ??= new ApiResult(statusCode, result.Exception);

        apiResult.AppendMeta(result.ToDictionary());

        return apiResult;
    }

    public static IApiResult ToApiResult<T>(this IUnitResult<T> result, string location)
    {
        var statusCode = GetStatusCode(result);

        ApiResult? apiResult = null;

        if (result.Result is not null && result.IsSuccess)
        {
            apiResult = new ApiResult<T>(result.Result, statusCode);

            if (result.Action == UnitAction.Add || result.Action == UnitAction.Update)
            {
                apiResult.AddHeader("Location", $"{location}/{result.Result}");
            }
            else if (result.Action == UnitAction.Pending)
            {
                apiResult.AddHeader("Status-Location", $"{location}/{result.Result}/status");
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
        var statusCode = GetStatusCode(result);
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
        var statusCode = GetStatusCode(result);

        if (result.IsSuccess && result.Result is not null)
        {
            // Handle collection
            if (result.Result.GetType().IsCollection(out var t))
            {
                var genericHyperMediaType = typeof(HypermediaApiListResult<>).MakeGenericType(t ?? throw new NullReferenceException("No generic type found"));
                var instance = Activator.CreateInstance(genericHyperMediaType, [result.Result, statusCode])
                    ?? throw new InvalidOperationException($"Failed to create instance of {nameof(HypermediaApiListResult<T>)}");
                var listResult = (ApiResult)instance;
                listResult.AppendMeta(result.ToDictionary());
                return listResult;
            }

            Console.WriteLine("Is Singleton Result, creating HypermediaApiResult");
            return ToHypermediaResultSingleton(result, location);
        }

        // Failure fallback
        var errorResult = new ApiResult(statusCode, result.Exception);
        errorResult.AppendMeta(result.ToDictionary());
        return errorResult;
    }

}
