using System.Collections.Concurrent;
using System.Collections.Immutable;
using NCDC.CommandParserBinder.MetaData;
using NCDC.Configuration.Metadatas;
using NCDC.Sharding.Routes.Abstractions;
using OpenConnector.Exceptions;

namespace NCDC.Sharding.Routes.DataSourceRoutes;

public sealed class DataSourceRouteManager:IDataSourceRouteManager
{
    private readonly ITableMetadataManager _tableMetadataManager;
    private readonly ILogicDatabase _logicDatabase;

    private readonly ConcurrentDictionary<string, IDataSourceRoute> _dataSourceRoutes = new ();

    public DataSourceRouteManager(ITableMetadataManager tableMetadataManager,ILogicDatabase logicDatabase)
    {
        _tableMetadataManager = tableMetadataManager;
        _logicDatabase = logicDatabase;
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
            return new List<string>() { _logicDatabase.DefaultDataSourceName };
        }
        var virtualDataSourceRoute = GetRoute(tableName);

        return virtualDataSourceRoute.Route(sqlParserResult);
    }

    public IReadOnlyCollection<IDataSourceRoute> GetRoutes()
    {
        return _dataSourceRoutes.Values.ToImmutableList();
    }

    public bool AddDataSourceRoute(IDataSourceRoute dataSourceRoute)
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