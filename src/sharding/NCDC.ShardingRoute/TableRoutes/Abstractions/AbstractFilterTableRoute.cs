using NCDC.Basic.Metadatas;
using NCDC.Plugin;
using NCDC.Plugin.TableRouteRules;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.DataSourceRoutes;

namespace NCDC.ShardingRoute.TableRoutes.Abstractions;

public abstract class AbstractFilterTableRoute:AbstractTableRoute
{

    protected AbstractFilterTableRoute(ITableRouteRule tableRouteRule,TableMetadata tableMetadata) : base(tableRouteRule,tableMetadata)
    {
    }

    public override ICollection<TableRouteUnit> Route(DataSourceRouteResult dataSourceRouteResult,SqlParserResult sqlParserResult)
    {
        var tableNames = GetTableMetadata().TableNames;
        var tableRouteRule = GetRouteRule();
        var beforeFilterTableName = tableRouteRule.BeforeFilterTableName(tableNames);
        var routeDataSource = Route0(dataSourceRouteResult,beforeFilterTableName,sqlParserResult);
        return tableRouteRule.AfterFilterTableName(tableNames, beforeFilterTableName, routeDataSource);
    }

    protected abstract ICollection<TableRouteUnit> Route0(DataSourceRouteResult dataSourceRouteResult,ICollection<string> beforeTableNames,SqlParserResult sqlParserResult);
}