using System.Collections.Concurrent;
using System.Collections.Immutable;
using NCDC.Basic.Configurations;
using NCDC.Basic.Metadatas;
using NCDC.Basic.TableMetadataManagers;
using NCDC.Exceptions;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.Abstractions;
using NCDC.ShardingRoute.DataSourceRoutes.Abstractions;

namespace NCDC.ShardingRoute.DataSourceRoutes;

public sealed class DataSourceRouteManager:IDataSourceRouteManager
{
    private readonly ITableMetadataManager _tableMetadataManager;
    private readonly ShardingConfiguration _shardingConfiguration;

    private readonly ConcurrentDictionary<string, IDataSourceRoute> _dataSourceRoutes = new ();

    public DataSourceRouteManager(ITableMetadataManager tableMetadataManager,ShardingConfiguration shardingConfiguration)
    {
        _tableMetadataManager = tableMetadataManager;
        _shardingConfiguration = shardingConfiguration;
    }
    public bool HasRoute(string tableName)
    {
        return _dataSourceRoutes.ContainsKey(tableName);
    }

    public IDataSourceRoute GetRoute(string tableName)
    {
        if (!_dataSourceRoutes.TryGetValue(tableName, out var dataSourceRoute))
        {
            throw new ShardingInvalidOperationException($"table:{tableName} not found {nameof(IDataSourceRoute)}");
        }

        return dataSourceRoute;
    }

    public ICollection<string> RouteTo(string tableName,SqlParserResult sqlParserResult)
    {
        if (!_tableMetadataManager.IsShardingDataSource(tableName))
        {
            return new List<string>() { _shardingConfiguration.DefaultDataSourceName };
        }
        var virtualDataSourceRoute = GetRoute(tableName);

        return virtualDataSourceRoute.Route(sqlParserResult);
    }

    public IReadOnlyCollection<IDataSourceRoute> GetRoutes()
    {
        return _dataSourceRoutes.Values.ToImmutableList();
    }

    public bool AddRoute(IDataSourceRoute dataSourceRoute)
    {
        var tableMetadata = _tableMetadataManager.TryGet(dataSourceRoute.TableName);
        if (tableMetadata == null)
        {
            throw new ShardingInvalidOperationException($"table:{dataSourceRoute.TableName} is not found table metadata");
        }
        if (!tableMetadata.IsMultiDataSourceMapping)
            throw new ShardingInvalidOperationException($"{dataSourceRoute.TableName} should configure sharding data source");

        return _dataSourceRoutes.TryAdd(dataSourceRoute.TableName, dataSourceRoute);

    }
}