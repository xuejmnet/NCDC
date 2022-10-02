using NCDC.Basic.TableMetadataManagers;
using NCDC.Enums;
using NCDC.MySqlParser;
using NCDC.ProxyServer;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Commons;
using NCDC.ProxyServer.Configurations.Apps;
using NCDC.ProxyServer.Configurations.Initializers;
using NCDC.ProxyServer.Contexts;
using NCDC.ProxyServer.Databases;
using NCDC.ProxyServer.DbProviderFactories;
using NCDC.ProxyServer.Executors;
using NCDC.ProxyServer.ServerDataReaders;
using NCDC.ProxyServer.ServerHandlers;
using NCDC.ShardingParser;
using NCDC.ShardingRewrite;
using NCDC.ShardingRoute;

namespace Microsoft.Extensions.DependencyInjection;

public static class DIExtension
{
    public static IServiceCollection AddProxyServerCore(this IServiceCollection services)
    {
        services.AddSingleton<IAppConfiguration, AppConfiguration>(sp =>
        {
            return new AppConfiguration(DatabaseTypeEnum.MySql, ConfigurationStorageTypeEnum.File, 3307, "/rule/test");
        });
        services.AddSingleton<IAppUserInitializer, DefaultAppUserInitializer>();
        services.AddSingleton<IAppInitializer, DefaultAppInitializer>();
        services.AddSingleton<IRoutePluginInitializer, DefaultRoutePluginInitializer>();
        services.AddSingleton<IInitializerManager, DefaultInitializerManager>();
        services.AddSingleton<IContextManager, DefaultContextManager>();
        services.AddSingleton<IServerHandlerFactory, ServerHandlerFactory>();
        services.AddSingleton<IServerDataReaderFactory, ServerDataReaderFactory>();
        services.AddSingleton<IAppUserManager, DefaultAppUserManager>();
        return services;
    }

    public static IServiceCollection AddInternalRuntimeContextService(this IServiceCollection services)
    {
        
        services.AddSingleton<IDbProviderFactory,ProxyDbProviderFactory>();
        services.AddSingleton<IVirtualDataSource,VirtualDataSource>();
        services.AddSingleton<ITableMetadataManager,TableMetadataManager>();
        services.AddSingleton<IShardingExecutionContextFactory,ShardingExecutionContextFactory>();
        services.AddShardingParser();
        services.AddMySqlParser();
        services.AddShardingRoute();
        services.AddShardingRewrite();
        return services;
    }
}