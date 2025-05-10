using IDFCR.Shared.Abstractions.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace IDFCR.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInjectableFilters<TTargetAssemblyClass>(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IFilterProvider<,>), typeof(FilterProvider<,>));
        
        services.Scan(c => c
            .FromAssemblyOf<TTargetAssemblyClass>()
            .AddClasses(i => i.AssignableTo<IInjectableFilter>())
            .AsImplementedInterfaces()
        );

#if DEBUG
        foreach(var service in services.Where(s => s.ServiceType == typeof(IInjectableFilter)))
        {
            Debug.WriteLine($"{service.ImplementationType}");
        }
#endif

        return services;
    }
}
