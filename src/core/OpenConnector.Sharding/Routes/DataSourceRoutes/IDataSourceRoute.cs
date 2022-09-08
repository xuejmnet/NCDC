using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Routes.DataSourceRoutes;

public interface IDataSourceRoute
{
    string TableName { get; }
    ICollection<string> Route(ISqlCommandContext<ISqlCommand> sqlCommandContext,ParameterContext parameterContext);
}