using NCDC.Basic.TableMetadataManagers;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.DataSourceRoutes;

namespace NCDC.ShardingRoute.TableRoutes.Abstractions;

public interface ITableRoute
{
    string TableName { get; }
    TableMetadata GetTableMetadata();
    ICollection<TableRouteUnit> Route(DataSourceRouteResult dataSourceRouteResult,SqlParserResult sqlParserResult);
}