using IDFCR.Shared.Http.Links;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IDFCR.Shared.Http.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLinkDependencies<TTargetAssemblyClass>(this IServiceCollection services)
    {
        services.AddSingleton<ILinkKeyDirective, DefaultLinkKeyDirective>();
        services.AddSingleton<ILinkKeyDirectiveOptions>(LinkKeyDirectiveOptions.Default);
        return services.Scan(c => c
            .FromAssemblyOf< TTargetAssemblyClass>()
            .AddClasses(i => i.AssignableTo<ILinkBuilder>())
            .AsImplementedInterfaces()
        );
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
