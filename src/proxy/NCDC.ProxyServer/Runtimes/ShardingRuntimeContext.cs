using Microsoft.Extensions.DependencyInjection;
using NCDC.Basic.Configurations;
using NCDC.Basic.TableMetadataManagers;
using NCDC.CommandParser.Abstractions;
using NCDC.Exceptions;
using NCDC.ProxyServer.Databases;
using NCDC.ProxyServer.Executors;
using NCDC.ProxyServer.Options;
using NCDC.ProxyServer.Runtimes.Initializer;
using NCDC.ProxyServer.ServiceProviders;
using NCDC.ShardingMerge.Abstractions;
using NCDC.ShardingRoute.Abstractions;

namespace NCDC.ProxyServer.Runtimes;

public sealed class ShardingRuntimeContext:IRuntimeContext
{
    private readonly IServiceProvider _serviceProvider;
    public string DatabaseName { get; }

    public ShardingRuntimeContext(string databaseName,IServiceProvider serviceProvider)
    {
        DatabaseName = databaseName;
        _serviceProvider = serviceProvider;
        InitFieldValue();
    }

    private ShardingConfiguration? _shardingConfiguration;
    public ShardingConfiguration GetShardingConfiguration()
    {
        return _shardingConfiguration??=GetRequiredService<ShardingConfiguration>();
    }

    private IRouteInitConfigOption? _routeInitConfigOption;
    public IRouteInitConfigOption GetRouteInitConfigOption()
    {
        return _routeInitConfigOption??=GetRequiredService<IRouteInitConfigOption>();
    }
    
    private IVirtualDataSource? _virtualDataSource;
    public IVirtualDataSource GetVirtualDataSource()
    {
        return _virtualDataSource??=GetRequiredService<IVirtualDataSource>();
    }

    private ITableMetadataInitializer? _tableMetadataInitializer;
    public ITableMetadataInitializer GetTableMetadataInitializer()
    {
        return _tableMetadataInitializer??=GetRequiredService<ITableMetadataInitializer>();
    }

    private ITableMetadataManager? _tableMetadataManager;
    public ITableMetadataManager GetTableMetadataManager()
    {
      return _tableMetadataManager??=GetRequiredService<ITableMetadataManager>();
    }

    private IDataSourceRouteManager? _dataSourceRouteManager;
    public IDataSourceRouteManager GetDataSourceRouteManager()
    {
        return _dataSourceRouteManager??=GetRequiredService<IDataSourceRouteManager>();
    }
    private ITableRouteManager? _tableRouteManager;
    public ITableRouteManager GetTableRouteManager()
    {
        return _tableRouteManager??=GetRequiredService<ITableRouteManager>();
    }

    private IShardingExecutionContextFactory? _shardingExecutionContextFactory;
    public IShardingExecutionContextFactory GetShardingExecutionContextFactory()
    {
        return _shardingExecutionContextFactory??=GetRequiredService<IShardingExecutionContextFactory>();
    }

    private IDataReaderMergerFactory? _dataReaderMergerFactory;
    public IDataReaderMergerFactory GetDataReaderMergerFactory()
    {
      return _dataReaderMergerFactory??=GetRequiredService<IDataReaderMergerFactory>();
    }

    // private ISqlCommandParser? _sqlCommandParser;
    // public ISqlCommandParser GetSqlCommandParser()
    // {
    //     return _sqlCommandParser??=GetRequiredService<ISqlCommandParser>();
    // }

    private IShardingProvider? _shardingProvider;
    public IShardingProvider GetShardingProvider()
    {
        return _shardingProvider??=GetRequiredService<IShardingProvider>();
    }


    public object? GetService(Type serviceType)
    {
        return _serviceProvider.GetService(serviceType);
    }

    public TService? GetService<TService>()
    {
        return _serviceProvider.GetService<TService>();
    }

    public object GetRequiredService(Type serviceType)
    {
        return _serviceProvider.GetRequiredService(serviceType);
    }

    public TService GetRequiredService<TService>() where TService : notnull
    {
        return _serviceProvider.GetRequiredService<TService>();
    }

    public object CreateInstance(Type serviceType,  params object[] parameters)
    {
        return ActivatorUtilities.CreateInstance(_serviceProvider,serviceType, parameters);
    }

    public TService CreateInstance<TService>(params object[] parameters)
    {
        return ActivatorUtilities.CreateInstance<TService>(_serviceProvider, parameters);
    }

    private void InitFieldValue()
    {
        GetShardingConfiguration();
        GetRouteInitConfigOption();
        GetVirtualDataSource();
        GetTableMetadataInitializer();
        GetTableMetadataManager();
        GetShardingExecutionContextFactory();
        GetDataReaderMergerFactory();
        // GetSqlCommandParser();
        GetDataSourceRouteManager();
        GetTableRouteManager();
    }
}