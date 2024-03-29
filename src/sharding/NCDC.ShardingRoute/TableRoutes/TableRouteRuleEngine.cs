using NCDC.ShardingParser;
using NCDC.ShardingRoute.Abstractions;
using NCDC.ShardingRoute.TableRoutes.Abstractions;
using NCDC.Extensions;
using NCDC.Plugin;
using NCDC.ShardingParser.MetaData;

namespace NCDC.ShardingRoute.TableRoutes;

public sealed class TableRouteRuleEngine : ITableRouteRuleEngine
{
    private readonly ITableMetadataManager _tableMetadataManager;
    private readonly ITableRouteManager _tableRouteManager;

    public TableRouteRuleEngine(ITableMetadataManager tableMetadataManager, ITableRouteManager tableRouteManager)
    {
        _tableMetadataManager = tableMetadataManager;
        _tableRouteManager = tableRouteManager;
    }

    private ICollection<TableRouteUnit> GetEntityRouteUnit(DataSourceRouteResult dataSourceRouteResult,
        string tableName, SqlParserResult sqlParserResult)
    {
        return _tableRouteManager.RouteTo(tableName, dataSourceRouteResult, sqlParserResult);
    }

    public ShardingRouteResult Route(TableRouteContext context)
    {
        Dictionary<string /*dataSourceName*/, Dictionary<string /*tableName*/, ISet<TableRouteUnit>>> routeMaps =
            new Dictionary<string, Dictionary<string, ISet<TableRouteUnit>>>();
        var tableNames = context.SqlParserResult.SqlCommandContext.GetTablesContext().GetTableNames();

        foreach (var tableName in tableNames)
        {
            if (!_tableMetadataManager.IsShardingTable(tableName))
            {
                continue;
            }

            var shardingRouteUnits =
                GetEntityRouteUnit(context.DataSourceRouteResult, tableName, context.SqlParserResult);

            foreach (var shardingRouteUnit in shardingRouteUnits)
            {
                var dataSourceName = shardingRouteUnit.DataSourceName;

                if (!routeMaps.TryGetValue(dataSourceName, out var tableNameMaps))
                {
                    tableNameMaps = new Dictionary<string, ISet<TableRouteUnit>>();
                    routeMaps.TryAdd(dataSourceName, tableNameMaps);
                }

                if (shardingRouteUnit.LogicTableName == shardingRouteUnit.ActualTableName)
                {
                    continue;
                }

                if (!tableNameMaps.TryGetValue(tableName, out var tableNameMapping))
                {
                    tableNameMapping = new HashSet<TableRouteUnit>();
                    tableNameMaps.TryAdd(tableName, tableNameMapping);
                }

                tableNameMapping.Add(shardingRouteUnit);
            }
        }

        //相同的数据源进行笛卡尔积
        //[[ds0,01,a],[ds0,02,a],[ds1,01,a]],[[ds0,01,b],[ds0,03,b],[ds1,01,b]]
        //=>
        //[ds0,[{01,a},{01,b}]],[ds0,[{01,a},{03,b}]],[ds0,[{02,a},{01,b}]],[ds0,[{02,a},{03,b}]],[ds1,[{01,a},{01,b}]]
        //如果笛卡尔积

        var sqlRouteUnits = new List<RouteUnit>(31);
        int dataSourceCount = 0;
        bool isCrossTable = false;
        foreach (var dataSourceName in context.DataSourceRouteResult.IntersectDataSources)
        {
            if (routeMaps.ContainsKey(dataSourceName))
            {
                var routeMap = routeMaps[dataSourceName];
                var tableRouteResults = routeMap.Select(o => o.Value).Cartesian()
                    .Select(o => new TableRouteResult(o.ToList())).Where(o => !o.IsEmpty).ToList();
                if (tableRouteResults.IsNotEmpty())
                {
                    dataSourceCount++;
                    if (tableRouteResults.Count > 1)
                    {
                        isCrossTable = true;
                    }

                    foreach (var tableRouteResult in tableRouteResults)
                    {
                        if (tableRouteResult.ReplaceTables.Count > 1)
                        {
                            isCrossTable = true;
                        }

                        var routeMappers = tableRouteResult.ReplaceTables
                            .Select(o => new RouteMapper(o.LogicTableName, o.ActualTableName)).ToList();
                        sqlRouteUnits.Add(new RouteUnit(dataSourceName, routeMappers));
                    }
                }
            }
        }

        return new ShardingRouteResult(sqlRouteUnits, sqlRouteUnits.Count == 0, dataSourceCount > 1, isCrossTable);
    }
}