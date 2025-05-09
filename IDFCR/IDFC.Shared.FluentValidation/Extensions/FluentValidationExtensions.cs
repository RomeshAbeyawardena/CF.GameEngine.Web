using Microsoft.Extensions.DependencyInjection;

namespace IDFC.Shared.FluentValidation.Extensions;

public static class FluentValidationExtensions
{
    public static void AddFluentValidationRequestPreProcessor(this MediatRServiceConfiguration services)
    {
        services.AddOpenRequestPreProcessor(typeof(FluentValidationRequestPreProcessor<>));
    }
}
