using Microsoft.Extensions.DependencyInjection;
using NCDC.Exceptions;
using NCDC.Plugin.DataSourceRouteRules;
using NCDC.Plugin.Extensions;
using NCDC.Plugin.TableRouteRules;
using NCDC.ProxyServer.Configurations.Initializers;
using NCDC.ProxyServer.Contexts.Initializers;
using NCDC.ProxyServer.Options;
using NCDC.ProxyServer.ServiceProviders;
using NCDC.ShardingRoute.DataSourceRoutes.Abstractions;
using NCDC.ShardingRoute.TableRoutes.Abstractions;

namespace NCDC.ProxyServer.Contexts;

public sealed class LogicDatabaseApplicationBuilder
{

    private readonly List<Action<IServiceCollection>> _serviceActions = new List<Action<IServiceCollection>>();

    private readonly IDictionary<string,Type> _tableRouteRules = new Dictionary<string, Type>();
    private readonly IDictionary<string,Type> _dataSourceRouteRules =new Dictionary<string, Type>();
    public string DatabaseName { get; }
    public IServiceCollection Services { get; }
    public ShardingConfigOption ConfigOption { get; }
    public IRouteInitConfigOption RouteConfigOption { get; }

    private LogicDatabaseApplicationBuilder(string databaseName)
    {
        DatabaseName = databaseName;
        ConfigOption = new ShardingConfigOption(databaseName);
        RouteConfigOption = new DefaultRouteInitConfigOption();
        Services = new ServiceCollection();
    }

    public static LogicDatabaseApplicationBuilder CreateBuilder(string databaseName)
    {
        return new LogicDatabaseApplicationBuilder(databaseName);
    }


    public LogicDatabaseApplicationBuilder AddTableRule(string logicTableName,Type type)
    {
        if (!type.IsTableRouteRule())
            throw new ShardingInvalidOperationException($"{type.FullName} is not implement {nameof(ITableRouteRule)}");

        if (!_tableRouteRules.ContainsKey(logicTableName))
            _tableRouteRules.TryAdd(logicTableName,type);

        return this;
    }

    public LogicDatabaseApplicationBuilder AddDataSourceRule(string logicTableName,Type type)
    {
        if (!type.IsDataSourceRouteRule())
            throw new ShardingInvalidOperationException(
                $"{type.FullName} is not implement {nameof(IDataSourceRouteRule)}");

        if (!_dataSourceRouteRules.ContainsKey(logicTableName))
            _dataSourceRouteRules.TryAdd(logicTableName,type);
        return this;
    }


    public LogicDatabaseApplicationBuilder AddServiceConfigure(Action<IServiceCollection> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        _serviceActions.Add(configure);
        ;
        return this;
    }

    public async Task<IRuntimeContext> BuildAsync(IServiceProvider appServiceProvider)
    {
        Services.AddSingleton<IShardingProvider>(sp => new ShardingProvider(sp, appServiceProvider));
        Services.AddSingleton<ShardingConfigOption>(ConfigOption);
        Services.AddSingleton<IRouteInitConfigOption>(RouteConfigOption);
        Services.AddSingleton<ITableMetadataInitializer,DefaultTableMetadataInitializer>();


        // Services.AddSingleton<IRuntimeContextInitializer, DefaultRuntimeContextInitializer>();
        Services.AddSingleton<IShardingProvider>(sp => new ShardingProvider(sp, appServiceProvider));
        Services.AddInternalRuntimeContextService();
        foreach (var serviceAction in _serviceActions)
        {
            serviceAction.Invoke(Services);
        }


        var shardingRuntimeContext = new ShardingRuntimeContext(DatabaseName, Services.BuildServiceProvider());
        await shardingRuntimeContext.Initialize();
        return shardingRuntimeContext;
    }
}