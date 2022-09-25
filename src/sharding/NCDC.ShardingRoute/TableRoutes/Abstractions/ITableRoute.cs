using NCDC.Basic.TableMetadataManagers;
using NCDC.Plugin;
using NCDC.Plugin.TableRouteRules;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.Abstractions;
using NCDC.ShardingRoute.DataSourceRoutes;

namespace NCDC.ShardingRoute.TableRoutes.Abstractions;

public interface ITableRoute:IRoute
{
    string TableName { get; }
    TableMetadata GetTableMetadata();
    ITableRouteRule GetRouteRule();
    ICollection<TableRouteUnit> Route(DataSourceRouteResult dataSourceRouteResult,SqlParserResult sqlParserResult);
}