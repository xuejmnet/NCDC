using NCDC.EntityFrameworkCore.Configurations;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Configurations;

namespace Microsoft.Extensions.DependencyInjection;

public static class DIExtension
{
    public static IServiceCollection AddEFCoreConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<IRuntimeContextBuilder, DbRuntimeContextBuilder>();
        services.AddSingleton<IShardingConfigOptionBuilder, DbShardingConfigOptionBuilder>();
        services.AddSingleton<IAppDatabaseConfiguration, DbAppDatabaseConfiguration>();
        return services;
    }
}