using IDFCR.Shared.Abstractions.Filters;
using IDFCR.Shared.Http.Links;
using IDFCR.Shared.Http.Mediatr;
using IDFCR.Shared.Http.Mediatr.Scopes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics;

namespace IDFCR.Shared.Http.Extensions;

public static class ServiceCollectionExtensions
{
    public static MediatRServiceConfiguration AddRoleRequirementPreProcessor(this MediatRServiceConfiguration configuration)
    {
        return configuration
            .AddOpenBehavior(typeof(RoleRequirementPrequestHandler<,>));
    }

    public static IServiceCollection AddCustomRoleRequirementHandlerInterceptor(this IServiceCollection services, Type serviceType)
    {
        if (!serviceType.IsGenericType || serviceType.GenericTypeArguments.Length != 2)
        {
            throw new InvalidCastException($"Service type {serviceType} is not a generic type.");
        }

        return services.AddScoped(typeof(IRoleRequirementHandlerInterceptor<>), serviceType);
    }

    public static IServiceCollection AddRoleRequirementServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IHttpContextWrapper, HttpContextWrapper>()
            .AddScoped<IScopedStateFactory, ScopedStateFactory>()
            .AddScoped<IScopedStateReader>(s => s.GetRequiredService<IScopedStateFactory>())
            .AddScoped<IScopedStateWriter>(s => s.GetRequiredService<IScopedStateFactory>())
            .AddCustomRoleRequirementHandlerInterceptor(typeof(ScopeStateRoleRequirementInterceptor<>));
    }

    public static IServiceCollection AddLinkDependencies<TTargetAssemblyClass>(this IServiceCollection services)
    {
        services.AddSingleton<ILinkKeyDirective, DefaultLinkKeyDirective>();
        services.AddSingleton<ILinkKeyDirectiveOptions>(LinkKeyDirectiveOptions.Default);
        services.Scan(c => c
            .FromAssemblyOf< TTargetAssemblyClass>()
            .AddClasses(i => i.AssignableTo<ILinkBuilder>())
            .AsImplementedInterfaces()
        );

#if DEBUG
        foreach (var service in services.Where(s => s.ServiceType == typeof(IInjectableFilter)))
        {
            Debug.WriteLine($"{service.ImplementationType}");
        }
#endif

        return services;
    }

    public static IServiceCollection ReplaceLinkKeyOptions<T>(this IServiceCollection services)
        where T : class, ILinkKeyDirectiveOptions
    {
        return services.Replace(new (typeof(ILinkKeyDirectiveOptions),typeof(T)));
    }

    public static IServiceCollection ReplaceLinkKeyDirective<T>(IServiceCollection services)
        where T : ILinkKeyDirective
    {
        return services.Replace(new(typeof(ILinkKeyDirective), typeof(T)));
    }
}
