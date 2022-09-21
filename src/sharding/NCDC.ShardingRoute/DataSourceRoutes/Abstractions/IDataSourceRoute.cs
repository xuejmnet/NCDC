using NCDC.Basic.TableMetadataManagers;
using NCDC.ShardingParser;
using NCDC.ShardingRoute.Abstractions;

namespace NCDC.ShardingRoute.DataSourceRoutes.Abstractions;

public interface IDataSourceRoute:IRouteDiscover
{
    string TableName { get; }
    TableMetadata GetTableMetadata();
    ICollection<string> Route(SqlParserResult sqlParserResult);
}