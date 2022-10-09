using Microsoft.Extensions.DependencyInjection;
using NCDC.Basic.Configurations;
using NCDC.Enums;
using NCDC.Exceptions;
using NCDC.Plugin.DataSourceRouteRules;
using NCDC.Plugin.Extensions;
using NCDC.Plugin.TableRouteRules;
using NCDC.ProxyServer.Options;
using NCDC.ProxyServer.Runtimes.Initializer;
using NCDC.ProxyServer.ServiceProviders;
using NCDC.ShardingMerge;
using NCDC.ShardingMerge.Abstractions;

namespace NCDC.ProxyServer.Runtimes.Builder;

public sealed class RuntimeApplicationBuilder
{

    private readonly List<Action<IServiceCollection>> _serviceActions = new List<Action<IServiceCollection>>();

    private readonly IDictionary<string,Type> _tableRouteRules = new Dictionary<string, Type>();
    private readonly IDictionary<string,Type> _dataSourceRouteRules =new Dictionary<string, Type>();
    public string DatabaseName { get; }
    public IServiceCollection Services { get; }
    public ShardingConfiguration ConfigOption { get; }
    public IRouteInitConfigOption RouteConfigOption { get; }

    private RuntimeApplicationBuilder(DatabaseTypeEnum databaseType,string databaseName)
    {
        DatabaseName = databaseName;
        ConfigOption = new ShardingConfiguration(databaseType,databaseName);
        RouteConfigOption = new DefaultRouteInitConfigOption();
        Services = new ServiceCollection();
    }

    public static RuntimeApplicationBuilder CreateBuilder(DatabaseTypeEnum databaseType,string databaseName)
    {
        return new RuntimeApplicationBuilder(databaseType,databaseName);
    }


    public RuntimeApplicationBuilder AddTableRule(string logicTableName,Type type)
    {
        if (!type.IsTableRouteRule())
            throw new ShardingInvalidOperationException($"{type.FullName} is not implement {nameof(ITableRouteRule)}");

        if (!_tableRouteRules.ContainsKey(logicTableName))
            _tableRouteRules.TryAdd(logicTableName,type);

        return this;
    }

    public RuntimeApplicationBuilder AddDataSourceRule(string logicTableName,Type type)
    {
        if (!type.IsDataSourceRouteRule())
            throw new ShardingInvalidOperationException(
                $"{type.FullName} is not implement {nameof(IDataSourceRouteRule)}");

        if (!_dataSourceRouteRules.ContainsKey(logicTableName))
            _dataSourceRouteRules.TryAdd(logicTableName,type);
        return this;
    }


    public RuntimeApplicationBuilder AddServiceConfigure(Action<IServiceCollection> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        _serviceActions.Add(configure);
        return this;
    }

    public IRuntimeContext Build(IServiceProvider appServiceProvider)
    {
        Services.AddSingleton<IShardingProvider>(sp => new ShardingProvider(sp, appServiceProvider));
        Services.AddSingleton<ShardingConfiguration>(ConfigOption);
        Services.AddSingleton<IRouteInitConfigOption>(RouteConfigOption);
        Services.AddSingleton<ITableMetadataInitializer,DefaultTableMetadataInitializer>();
        Services.AddSingleton<IDataReaderMergerFactory,DataReaderMergerFactory>();


        // Services.AddSingleton<IRuntimeInitializer, DefaultRuntimeContextInitializer>();
        Services.AddSingleton<IShardingProvider>(sp => new ShardingProvider(sp, appServiceProvider));
        Services.AddInternalRuntimeContextService();
        foreach (var serviceAction in _serviceActions)
        {
            serviceAction.Invoke(Services);
        }


        var shardingRuntimeContext = new ShardingRuntimeContext(DatabaseName, Services.BuildServiceProvider());
        return shardingRuntimeContext;
    }
}