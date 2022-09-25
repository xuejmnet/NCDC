using NCDC.Basic.TableMetadataManagers;
using NCDC.Plugin;
using NCDC.Plugin.DataSourceRouteRules;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.Abstractions;

namespace NCDC.ShardingRoute.DataSourceRoutes.Abstractions;

public interface IDataSourceRoute:IRoute
{
    string TableName { get; }
    TableMetadata GetTableMetadata();
    IDataSourceRouteRule GetRouteRule();
    ICollection<string> Route(SqlParserResult sqlParserResult);
}