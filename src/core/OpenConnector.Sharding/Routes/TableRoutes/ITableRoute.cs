using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.Sharding.Routes.Abstractions;
using OpenConnector.Sharding.Routes.DataSourceRoutes;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Routes.TableRoutes;

public interface ITableRoute
{
    string TableName { get; }
    TableMetadata GetTableMetadata();
    ICollection<TableRouteUnit> Route(DataSourceRouteResult dataSourceRouteResult,SqlParserResult sqlParserResult);
}