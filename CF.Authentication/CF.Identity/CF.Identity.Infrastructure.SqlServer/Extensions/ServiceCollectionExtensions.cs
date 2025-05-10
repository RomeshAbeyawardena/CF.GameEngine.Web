using CF.Identity.Infrastructure.Features.AccessToken;
using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.SqlServer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Extensions;

namespace CF.Identity.Infrastructure.SqlServer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackendDependencies(this IServiceCollection services, string connectionName)
    {
        services.AddDbContext<CFIdentityDbContext>((s, opt) =>
        {
            var connectionString = s.GetRequiredService<IConfiguration>().GetConnectionString(connectionName);
            opt.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging();
        });

        return services
            .AddSingleton<IClientCredentialHasher, ClientCredentialHasher>()
            .AddScoped<IAccessTokenRepository, AccessTokenRepository>()
            .AddScoped<IClientRepository, ClientRepository>()
            .AddScoped<IScopeRepository, ScopeRepository>()
            .AddInjectableFilters<CFIdentityDbContext>();
    }
}
