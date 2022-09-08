using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Abstractions;

public abstract class AbstractDataSourceRoute:IDataSourceRoute,ITableMetadataAutoBindInitializer
{
    
    public abstract ICollection<string> GetDataSourceNames();
    
    public abstract bool AddDataSourceName(string dataSourceName);

    public abstract string TableName { get; }
    protected TableMetadata TableMetadata { get; private set; }
    public abstract ICollection<string> Route(ISqlCommandContext<ISqlCommand> sqlCommandContext,ParameterContext parameterContext);
    public void Initialize(TableMetadata tableMetadata)
    {
        TableMetadata = tableMetadata;
    }
}