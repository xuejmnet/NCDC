using Microsoft.Extensions.Configuration;
using NCDC.Enums;
using NCDC.Extensions;
using NCDC.MySqlParser;
using NCDC.ProxyServer;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.AppServices;
using NCDC.ProxyServer.AppServices.Abstractions;
using NCDC.ProxyServer.Bootstrappers;
using NCDC.ProxyServer.Commons;
using NCDC.ProxyServer.Connection;
using NCDC.ProxyServer.Databases;
using NCDC.ProxyServer.DbProviderFactories;
using NCDC.ProxyServer.Executors;
using NCDC.ProxyServer.Runtimes.Builder;
using NCDC.ProxyServer.ServerDataReaders;
using NCDC.ProxyServer.ServerHandlers;
using NCDC.ShardingParser;
using NCDC.ShardingParser.Abstractions;
using NCDC.ShardingParser.MetaData;
using NCDC.ShardingRewrite;
using NCDC.ShardingRoute;

namespace Microsoft.Extensions.DependencyInjection;

public static class DIExtension
{
    public static DatabaseTypeEnum ParseDatabaseType(this string databaseType)
    {
        if ("MySql".EqualsIgnoreCase(databaseType))
        {
            return DatabaseTypeEnum.MySql;
        }

        throw new NotImplementedException(databaseType);
    }
    public static DbStorageTypeEnum ParseStorageType(this string storageType)
    {
        if ("MySql".EqualsIgnoreCase(storageType))
        {
            return DbStorageTypeEnum.MySql;
        }

        throw new NotImplementedException(storageType);
    }
    public static int ParseInt(this string portStr)
    {
        if (!int.TryParse(portStr,out var port ))
        {
            throw new NotImplementedException(portStr);
        }

        return port;
    }
    public static bool ParseBool(this string portStr)
    {
        if (!bool.TryParse(portStr,out var b ))
        {
            throw new NotImplementedException(portStr);
        }

        return b;
    }
    public static IServiceCollection AddProxyServerCore(this IServiceCollection services)
    {
        services.AddSingleton<IAppConfiguration, AppConfiguration>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var database = configuration["DatabaseType"];
            var storage = configuration["StorageType"];
            var connectionString = configuration["ConnectionString"];
            var port = configuration["Port"];
            var routePluginPath = configuration["RoutePluginPath"];
            var logEncode = configuration["LogEncode"];
            var logDecode = configuration["LogDecode"];
            return new AppConfiguration()
            {
               DatabaseType = database.ParseDatabaseType(),
               StorageType = storage.ParseStorageType(),
               ConnectionsString = connectionString,
               Port = port.ParseInt(),
               RulePluginPath = routePluginPath,
               LogEncode = logEncode.ParseBool(),
               LogDecode = logDecode.ParseBool()
            };
        });
        // services.AddSingleton<IAppConfiguration, AppConfiguration>(sp =>
        // {
        //     var configuration = sp.GetService<IConfiguration>();
        //     return new AppConfiguration();
        // });
        services.AddSingleton<IAppBootstrapper, AppBootstrapper>();
        //IAppRuntimeLoader,IAppUserLoader,IUserDatabaseMappingLoader
        services.AddSingleton<IConnectionSessionFactory, DefaultConnectionSessionFactory>();
        services.AddSingleton<IAppRuntimeManager, DefaultAppAppRuntimeManager>();
        services.AddSingleton<IAppRuntimeLoader>(sp=>(sp.GetService<IAppRuntimeManager>() as IAppRuntimeLoader)!);
        services.AddSingleton<IAppUserLoader>(sp=>(sp.GetService<IAppRuntimeManager>() as IAppUserLoader)!);
        services.AddSingleton<IUserDatabaseMappingLoader>(sp=>(sp.GetService<IAppRuntimeManager>() as IUserDatabaseMappingLoader)!);
        services.AddSingleton<IServerHandlerFactory, ServerHandlerFactory>();
        services.AddSingleton<IServerDataReaderFactory, ServerDataReaderFactory>();
        return services;
    }
    
    public static IServiceCollection AddAppService<TBuilder,TInitializer>(this IServiceCollection services)
        where TBuilder:class,IAppRuntimeBuilder
        where TInitializer:class,IAppInitializer
    {
        services.AddSingleton<IAppInitializer, TInitializer>();
        services.AddSingleton<IAppRuntimeBuilder, TBuilder>();
        return services;
    }


    

    public static IServiceCollection AddInternalRuntimeContextService(this IServiceCollection services)
    {
        
        services.AddSingleton<IDbProviderFactory,ProxyDbProviderFactory>();
        services.AddSingleton<IVirtualDataSource,VirtualDataSource>();
        services.AddSingleton<ITableMetadataManager,TableMetadataManager>();
        services.AddSingleton<IShardingExecutionContextFactory,ShardingExecutionContextFactory>();
        // services.AddSingleton<ISqlCommandContextFactory, SqlCommandContextFactory>();
        // services.AddShardingParser();
        services.AddShardingRoute();
        services.AddShardingRewrite();
        return services;
    }
}