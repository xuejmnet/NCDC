using System.Collections.Concurrent;
using System.Collections.Immutable;
using NCDC.Basic.Metadatas;
using NCDC.Basic.TableMetadataManagers;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.Abstractions;
using NCDC.ShardingRoute.DataSourceRoutes;
using NCDC.ShardingRoute.TableRoutes.Abstractions;
using OpenConnector.Exceptions;

namespace NCDC.ShardingRoute.TableRoutes;

public sealed class TableRouteManager : ITableRouteManager
{
    private readonly ITableMetadataManager _tableMetadataManager;
    private readonly ILogicDatabase _logicDatabase;
    private readonly ConcurrentDictionary<string, ITableRoute> _tableRoutes = new();

    public TableRouteManager(ITableMetadataManager tableMetadataManager, ILogicDatabase logicDatabase)
    {
        _tableMetadataManager = tableMetadataManager;
        _logicDatabase = logicDatabase;
    }

    public bool HasRoute(string tableName)
    {
        return _tableRoutes.ContainsKey(tableName);
    }

    public ITableRoute GetRoute(string tableName)
    {
        if (!_tableRoutes.TryGetValue(tableName, out var tableRoute))
        {
            throw new ShardingInvalidOperationException($"table:{tableName} not found {nameof(ITableRoute)}");
        }

        return tableRoute;
    }

    public IReadOnlyCollection<ITableRoute> GetRoutes()
    {
        return _tableRoutes.Values.ToImmutableList();
    }

    public bool AddRoute(ITableRoute route)
    {
        var tableMetadata = _tableMetadataManager.TryGet(route.TableName);
        if (tableMetadata == null)
        {
            throw new ShardingInvalidOperationException($"table:{route.TableName} is not found table metadata");
        }

        if (!tableMetadata.IsMultiTableMapping)
            throw new ShardingInvalidOperationException($"{route.TableName} should configure sharding table");

        return _tableRoutes.TryAdd(route.TableName, route);
    }

    public ICollection<TableRouteUnit> RouteTo(string tableName, DataSourceRouteResult dataSourceRouteResult,
        SqlParserResult sqlParserResult)
    {
        var route = GetRoute(tableName);
        return route.Route(dataSourceRouteResult, sqlParserResult);
    }
}