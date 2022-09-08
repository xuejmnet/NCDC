using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Abstractions;

public interface IDataSourceRoute
{
    string TableName { get; }
    ICollection<string> Route(ISqlCommandContext<ISqlCommand> sqlCommandContext,ParameterContext parameterContext);
}