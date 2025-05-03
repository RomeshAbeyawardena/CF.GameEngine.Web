using IDFCR.Shared.Exceptions;
using IDFCR.Shared.Http.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IDFCR.Shared.Http.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context); // Process the next piece of middleware
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred.");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            IExposableException => StatusCodes.Status400BadRequest, // Business / safe client error
            KeyNotFoundException => StatusCodes.Status404NotFound,  // Common lookup failures
            _ => StatusCodes.Status500InternalServerError           // Unexpected server failure
        };

        var apiResult = new ApiResult(statusCode, exception);

        await apiResult.ExecuteAsync(context);
    }
}
