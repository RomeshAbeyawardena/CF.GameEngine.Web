using CF.Identity.Infrastructure.Features.AccessRoles;
using CF.Identity.Infrastructure.Features.AccessToken;
using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.Features.Scope;
using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer.PII;
using CF.Identity.Infrastructure.SqlServer.Protection;
using CF.Identity.Infrastructure.SqlServer.Repositories;
using CF.Identity.Infrastructure.SqlServer.SPA;
using IDFCR.Shared.EntityFramework;
using IDFCR.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Security.Cryptography;
using System.Text;

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

        services.TryAddSingleton(TimeProvider.System);

        return services
            .AddSingleton(Encoding.UTF8)
            .AddTransient((x) => RandomNumberGenerator.Create())
            .AddSingleton<IJwtSettings, ConfigurationDerivedJwtSettings>()
            .AddScoped<IAccessRoleRepository, AccessRoleRepository>()
            .AddScoped<IAccessTokenProtection, AccessTokenProtection>()
            .AddScoped<IAccessTokenSpaProtection, AccessTokenProtection>()
            .AddScoped<IAccessTokenRepository, AccessTokenRepository>()

            .AddScoped<ICommonNameRepository, CommonNameRepository>()
            .AddScoped<ICommonNamePIIProtection, CommonNamePIIProtection>()
            .AddScoped<ITransactionalUnitOfWork>(x => x.GetRequiredService<IAccessTokenRepository>() as AccessTokenRepository ?? throw new InvalidCastException())

            .AddScoped<IClientProtection, ClientProtection>()
            .AddScoped<IClientSpaProtection, ClientProtection>()

            .AddScoped<IClientRepository, ClientRepository>()
            .AddScoped<IScopeRepository, ScopeRepository>()
            .AddScoped<IUserPIIProtection, UserPIIProtection>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddInjectableFilters<CFIdentityDbContext>();
    }
}
