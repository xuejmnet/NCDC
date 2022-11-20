using Microsoft.Extensions.Logging;
using NCDC.Basic.Metadatas;
using NCDC.Basic.Plugin;
using NCDC.Helpers;
using NCDC.Logger;
using NCDC.Plugin;
using NCDC.Plugin.DataSourceRouteRules;
using NCDC.Plugin.TableRouteRules;
using NCDC.ProxyServer.Extensions;
using NCDC.ProxyServer.Options;
using NCDC.ProxyServer.ServiceProviders;
using NCDC.ShardingRoute.Abstractions;
using NCDC.ShardingRoute.DataSourceRoutes.Abstractions;
using NCDC.ShardingRoute.TableRoutes.Abstractions;

namespace NCDC.ProxyServer.Runtimes.Initializer;

public class DefaultTableMetadataInitializer : ITableMetadataInitializer
{
    private static readonly ILogger<DefaultTableMetadataInitializer> _logger =NCDCLoggerFactory.CreateLogger<DefaultTableMetadataInitializer>();
    private readonly IShardingProvider _serviceProvider;
    private readonly IRouteInitConfigOption _routeInitConfigOption;
    private readonly IDataSourceRouteManager _dataSourceRouteManager;
    private readonly ITableRouteManager _tableRouteManager;

    public DefaultTableMetadataInitializer(IShardingProvider shardingProvider,
        IRouteInitConfigOption routeInitConfigOption,IDataSourceRouteManager dataSourceRouteManager,ITableRouteManager tableRouteManager)
    {
        _serviceProvider = shardingProvider;
        _routeInitConfigOption = routeInitConfigOption;
        _dataSourceRouteManager = dataSourceRouteManager;
        _tableRouteManager = tableRouteManager;
    }

    public Task<bool> InitializeAsync(TableMetadata tableMetadata)
    {
        var tableConfiguration = new TableConfiguration(tableMetadata.LogicTableName);
        if (_routeInitConfigOption.HasDataSourceRouteRule(tableMetadata.LogicTableName))
        {
            var routeRuleTypeName = _routeInitConfigOption.GetDataSourceRouteRule(tableMetadata.LogicTableName);
            var routeRuleType = RuntimeHelper.GetAllTypes()
                .FirstOrDefault(o => Equals(o.FullName, routeRuleTypeName));
            if (routeRuleType == null)
            {
                _logger.LogWarning($"table:[{tableMetadata.LogicTableName}],data source rule:[{routeRuleTypeName}] not found");
                return Task.FromResult(false);
            }
            var routeRule = (IDataSourceRouteRule)_serviceProvider.CreateInstance(routeRuleType);
            if (routeRule is IDataSourceRuleConfigure dataSourceRuleConfigure)
            {
                var builder = DataSourceRuleConfigureBuilder.CreateDataSourceRuleConfigureBuilder(tableConfiguration);

                dataSourceRuleConfigure.Configure(builder);
                if (tableConfiguration.ShardingDataSourceColumn == null)
                {
                    _logger.LogWarning($"table:[{tableMetadata.LogicTableName}],data source rule:[{routeRuleTypeName}] sharding data source column is null");
                    return Task.FromResult(false);
                }
                tableMetadata.SetShardingDataSourceColumn(tableConfiguration.ShardingDataSourceColumn);
                foreach (var column in tableConfiguration.ShardingDataSourceColumns)
                {
                    if (column != tableConfiguration.ShardingDataSourceColumn)
                    {
                        tableMetadata.AddExtraSharingDataSourceColumn(column);
                    }
                }
            }


            var shardingDataSourceRoute = new ShardingDataSourceRoute(routeRule,tableMetadata);
            _dataSourceRouteManager.AddRoute(shardingDataSourceRoute);
        }

        if (_routeInitConfigOption.HasTableRouteRule(tableMetadata.LogicTableName))
        {
            var routeRuleTypeName = _routeInitConfigOption.GetTableRouteRule(tableMetadata.LogicTableName);
            // foreach (var allType in RuntimeHelper.GetAllTypes())
            // {
            //     Console.WriteLine(allType.FullName);
            // }
            // var assembly = PluginLoader.Load("ShardingRoutePluginTest.dll");
            // var type = assembly.GetType(routeRuleTypeName);
            var routeRuleType = PluginLoader.Instance.Assemblies
                                    .Select(o=>o.GetType(routeRuleTypeName))
                                    .FirstOrDefault(o => o!=null);
            if (routeRuleType == null)
            {
                _logger.LogWarning($"table:[{tableMetadata.LogicTableName}],table rule:[{routeRuleTypeName}] not found");
                return Task.FromResult(false);
            }
            var routeRule = (ITableRouteRule)_serviceProvider.CreateInstance(routeRuleType);
            if (routeRule is ITableRuleConfigure tableRuleConfigure)
            {
                var builder = TableRuleConfigureBuilder.CreateTableRuleConfigureBuilder(tableConfiguration);

                tableRuleConfigure.Configure(builder);
                if (tableConfiguration.ShardingTableColumn == null)
                {
                    _logger.LogWarning($"table:[{tableMetadata.LogicTableName}],table rule:[{routeRuleTypeName}] sharding table column is null");
                    return Task.FromResult(false);
                }
                tableMetadata.SetShardingTableColumn(tableConfiguration.ShardingTableColumn);
                foreach (var column in tableConfiguration.ShardingTableColumns)
                {
                    if (column != tableConfiguration.ShardingTableColumn)
                    {
                        tableMetadata.AddExtraSharingTableColumn(column);
                    }
                }
            }

            var shardingTableRoute = new ShardingTableRoute(routeRule,tableMetadata);
            _tableRouteManager.AddRoute(shardingTableRoute);
        }

        return Task.FromResult(true);
    }
}