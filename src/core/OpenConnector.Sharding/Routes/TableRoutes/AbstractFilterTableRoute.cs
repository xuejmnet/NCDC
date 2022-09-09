using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.Sharding.Routes.Abstractions;
using OpenConnector.Sharding.Routes.DataSourceRoutes;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Routes.TableRoutes;

public abstract class AbstractFilterTableRoute:AbstractTableRoute
{
    public AbstractFilterTableRoute(ITableMetadataManager tableMetadataManager) : base(tableMetadataManager)
    {
    }

    public override ICollection<TableRouteUnit> Route(DataSourceRouteResult dataSourceRouteResult,SqlParserResult sqlParserResult)
    {
        var tableNames = GetTableMetadata().TableNames;
        var beforeTableNames = BeforeFilterTableName(tableNames);
        var routeDataSource = Route0(dataSourceRouteResult,beforeTableNames,sqlParserResult);
        return AfterFilterTableName(tableNames, beforeTableNames, routeDataSource);
    }

    protected abstract ICollection<TableRouteUnit> Route0(DataSourceRouteResult dataSourceRouteResult,ICollection<string> beforeTableNames,SqlParserResult sqlParserResult);

    protected virtual ICollection<string> BeforeFilterTableName(ICollection<string> allDataSource)
    {
        return allDataSource;
    }
    protected virtual ICollection<TableRouteUnit> AfterFilterTableName(ICollection<string> allTableNames,ICollection<string> beforeTableNames,
        ICollection<TableRouteUnit> filterRouteUnits)
    {
        return filterRouteUnits;
    }
}