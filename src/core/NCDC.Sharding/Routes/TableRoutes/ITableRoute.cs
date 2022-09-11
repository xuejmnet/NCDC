using NCDC.CommandParser.Abstractions;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.MetaData;
using NCDC.Basic.TableMetadataManagers;
using NCDC.Sharding.Routes.Abstractions;
using NCDC.Sharding.Routes.DataSourceRoutes;
using NCDC.ShardingAdoNet;

namespace NCDC.Sharding.Routes.TableRoutes;

public interface ITableRoute
{
    string TableName { get; }
    TableMetadata GetTableMetadata();
    ICollection<TableRouteUnit> Route(DataSourceRouteResult dataSourceRouteResult,SqlParserResult sqlParserResult);
}