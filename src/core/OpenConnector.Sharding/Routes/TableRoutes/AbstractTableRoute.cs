using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.Exceptions;
using OpenConnector.Sharding.Abstractions;
using OpenConnector.Sharding.Routes.Abstractions;
using OpenConnector.Sharding.Routes.DataSourceRoutes;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Routes.TableRoutes;

public abstract class AbstractTableRoute:ITableRoute
{
    private readonly TableMetadata _tableMetadata;
    protected AbstractTableRoute(ITableMetadataManager tableMetadataManager)
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

    public abstract ICollection<TableRouteUnit> Route(DataSourceRouteResult dataSourceRouteResult,SqlParserResult sqlParserResult);

}