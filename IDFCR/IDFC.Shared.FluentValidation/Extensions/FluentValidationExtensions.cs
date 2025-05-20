using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace IDFCR.Shared.FluentValidation.Extensions;

public static class FluentValidationExtensions
{
    /// <summary>
    /// Add this before you add <see cref="AddMediatr"/> in your service registration
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddGenericExceptionHandler(this IServiceCollection services)
    {
        return services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(UnitExceptionHandler<,,>));
    }

    public static void AddFluentValidationRequestPreProcessor(this MediatRServiceConfiguration services)
    {
        services
            .AddOpenRequestPreProcessor(typeof(FluentValidationRequestPreProcessor<>));
    }
}
