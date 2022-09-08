using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.Sharding.Abstractions;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Routes.TableRoutes;

public abstract class AbstractTableRoute:ITableRoute,ITableMetadataAutoBindInitializer
{
    public abstract string TableName { get; }
    public ICollection<RouteUnit> Route(ISqlCommandContext<ISqlCommand> sqlCommandContext, ParameterContext parameterContext)
    {
        throw new NotImplementedException();
    }

    public void Initialize(TableMetadata tableMetadata)
    {
        throw new NotImplementedException();
    }
}