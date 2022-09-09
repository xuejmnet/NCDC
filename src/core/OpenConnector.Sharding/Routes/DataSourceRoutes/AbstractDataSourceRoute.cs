using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.Exceptions;
using OpenConnector.Sharding.Routes.Abstractions;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Routes.DataSourceRoutes;

public abstract class AbstractDataSourceRoute:IDataSourceRoute
{
    private readonly TableMetadata _tableMetadata;
    protected AbstractDataSourceRoute(ITableMetadataManager tableMetadataManager)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        var tableName = TableName;
        _tableMetadata = tableMetadataManager.TryGet(tableName) ??
                         throw new ShardingConfigException($"cant find table metadata:{tableName}");
    }

    public abstract string TableName { get; }
    public TableMetadata GetTableMetadata()
    {
        return _tableMetadata;
    }

    public abstract ICollection<string> Route(SqlParserResult sqlParserResult);

}