using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Routes.TableRoutes;

public interface ITableRoute
{
    string TableName { get; }
    ICollection<RouteUnit> Route(ISqlCommandContext<ISqlCommand> sqlCommandContext,ParameterContext parameterContext);
}