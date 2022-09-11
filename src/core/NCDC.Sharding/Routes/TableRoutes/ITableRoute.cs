using NCDC.CommandParser.Abstractions;
using NCDC.Basic.Parser.Command;
using NCDC.Basic.Parser.MetaData;
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