using NCDC.Basic.Parser.MetaData;
using NCDC.Basic.TableMetadataManagers;
using OpenConnector.Exceptions;
using NCDC.Sharding.Routes.DataSourceRoutes;

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