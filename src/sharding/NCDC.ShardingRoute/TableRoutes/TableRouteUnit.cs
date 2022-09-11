namespace NCDC.ShardingRoute.TableRoutes;

public sealed class TableRouteUnit
{
    public string DataSourceName { get; }
    public string LogicTableName { get; }
    public string ActualTableName { get; }

    public TableRouteUnit(string dataSourceName,string logicTableName,string actualTableName)
    {
        DataSourceName = dataSourceName;
        LogicTableName = logicTableName;
        ActualTableName = actualTableName;
    }
}