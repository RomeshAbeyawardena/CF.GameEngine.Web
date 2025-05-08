using CF.GameEngine.Infrastructure.Features.Elements;
using CF.GameEngine.Infrastructure.Features.ElementTypes;
using CF.GameEngine.Infrastructure.SqlServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CF.GameEngine.Infrastructure.SqlServer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackendDependencies(this IServiceCollection services, string connectionName)
    {
        services.AddDbContext<CFGameEngineDbContext>((s, opt) =>
        {
            var connectionString = s.GetRequiredService<IConfiguration>().GetConnectionString(connectionName);
            opt
                .UseSqlServer(connectionString)
                .EnableSensitiveDataLogging();
        });

        return services
            .AddScoped<IElementTypeRepository, ElementTypeRepository>()
            .AddScoped<IElementRepository, ElementRepository>();
    }
}
