using NCDC.ShardingParser.MetaData;
using NCDC.Basic.TableMetadataManagers;

namespace NCDC.Sharding.Routes.DataSourceRoutes;

public abstract class AbstractFilterDataSourceRoute:AbstractDataSourceRoute
{
    protected AbstractFilterDataSourceRoute(ITableMetadataManager tableMetadataManager) : base(tableMetadataManager)
    {
    }
    public override ICollection<string> Route(SqlParserResult sqlParserResult)
    {
        var dataSourceNames = GetTableMetadata().DataSources;
        var beforeDataSources = BeforeFilterDataSource(dataSourceNames);
        var routeDataSource = Route0(beforeDataSources,sqlParserResult);
        return AfterFilterDataSource(dataSourceNames, beforeDataSources, routeDataSource);
    }

    protected abstract ICollection<string> Route0(ICollection<string> beforeDataSources,SqlParserResult sqlParserResult);

    protected virtual ICollection<string> BeforeFilterDataSource(ICollection<string> allDataSource)
    {
        return allDataSource;
    }
    protected virtual ICollection<string> AfterFilterDataSource(ICollection<string> allDataSources,ICollection<string> beforeDataSources,
        ICollection<string> filterDataSources)
    {
        return filterDataSources;
    }

}