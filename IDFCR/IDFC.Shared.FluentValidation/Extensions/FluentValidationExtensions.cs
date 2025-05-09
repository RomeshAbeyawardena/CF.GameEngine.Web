using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace IDFC.Shared.FluentValidation.Extensions;

public static class FluentValidationExtensions
{
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
