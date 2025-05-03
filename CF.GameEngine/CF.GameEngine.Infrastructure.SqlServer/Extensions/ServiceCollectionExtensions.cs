using CF.GameEngine.Infrastructure.Features.ElementTypes;
using CF.GameEngine.Infrastructure.SqlServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CF.GameEngine.Infrastructure.SqlServer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackendDependencies(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CFGameEngineDbContext>((s, opt) =>
            opt.UseSqlServer(s.GetRequiredService<IConfiguration>().GetConnectionString(connectionString)));
        return services.AddScoped<IElementTypeRepository, ElementTypeRepository>();
    }
}
