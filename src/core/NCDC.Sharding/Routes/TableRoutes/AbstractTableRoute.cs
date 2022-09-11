using NCDC.CommandParser.Abstractions;
using NCDC.CommandParserBinder.Command;
using NCDC.CommandParserBinder.MetaData;
using OpenConnector.Exceptions;
using NCDC.Sharding.Abstractions;
using NCDC.Sharding.Routes.Abstractions;
using NCDC.Sharding.Routes.DataSourceRoutes;
using NCDC.ShardingAdoNet;

namespace NCDC.Sharding.Routes.TableRoutes;

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