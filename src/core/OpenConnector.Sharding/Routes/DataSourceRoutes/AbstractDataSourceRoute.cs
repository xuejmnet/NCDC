using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.Sharding.Abstractions;
using OpenConnector.Sharding.Routes.Abstractions;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Routes.DataSourceRoutes;

public abstract class AbstractDataSourceRoute:IDataSourceRoute,ITableMetadataAutoBindInitializer
{
    public abstract string TableName { get; }
    protected TableMetadata TableMetadata { get; private set; }
    public abstract ICollection<string> Route(ISqlCommandContext<ISqlCommand> sqlCommandContext,ParameterContext parameterContext);
    public void Initialize(TableMetadata tableMetadata)
    {
        TableMetadata = tableMetadata;
    }
}