using NCDC.Basic.Configurations;
using NCDC.Exceptions;
using NCDC.ShardingRoute.Abstractions;
using NCDC.ShardingRoute.DataSourceRoutes.Abstractions;
using NCDC.Extensions;
using NCDC.Plugin;
using NCDC.ShardingParser.MetaData;

namespace NCDC.ShardingRoute.DataSourceRoutes;

public sealed class DataSourceRouteRuleEngine:IDataSourceRouteRuleEngine
{
    private readonly ITableMetadataManager _tableMetadataManager;
    private readonly ShardingConfiguration _shardingConfiguration;
    private readonly IDataSourceRouteManager _dataSourceRouteManager;

    public DataSourceRouteRuleEngine(ITableMetadataManager tableMetadataManager,ShardingConfiguration shardingConfiguration,IDataSourceRouteManager dataSourceRouteManager)
    {
        _tableMetadataManager = tableMetadataManager;
        _shardingConfiguration = shardingConfiguration;
        _dataSourceRouteManager = dataSourceRouteManager;
    }
    public DataSourceRouteResult Route(DataSourceRouteRuleContext context)
    {
        var dataSourceMaps = new Dictionary<string, ISet<string>>();

        var tableNames = context.SqlParserResult.SqlCommandContext.GetTablesContext().GetTableNames();
        foreach (var tableName in tableNames)
        {
            if (!_tableMetadataManager.IsShardingDataSource(tableName))
            {
                if (!dataSourceMaps.TryGetValue(tableName, out var ds))
                {
                    dataSourceMaps.Add(tableName, new HashSet<string>() { _shardingConfiguration.DefaultDataSourceName });
                }
                continue;
            }
            var dataSources = _dataSourceRouteManager.RouteTo(tableName, context.SqlParserResult);
            if (!dataSourceMaps.ContainsKey(tableName))
            {
                dataSourceMaps.Add(tableName, dataSources.ToHashSet());
            }
            else
            {
                foreach (var shardingDataSource in dataSources)
                {
                    dataSourceMaps[tableName].Add(shardingDataSource);
                }
            }
        }

        if (dataSourceMaps.IsEmpty())
            throw new ShardingException(
                $"data source route not match: {context.SqlParserResult.Sql}");
        if (dataSourceMaps.Count == 1)
            return new DataSourceRouteResult(dataSourceMaps.First().Value);
        var intersect = dataSourceMaps.Select(o => o.Value).Aggregate((p, n) => p.Intersect(n).ToHashSet());
        return new DataSourceRouteResult(intersect);
    }
}