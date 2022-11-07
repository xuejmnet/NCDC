using NCDC.Basic.Metadatas;
using NCDC.Plugin.DataSourceRouteRules;
using NCDC.ShardingParser;

namespace NCDC.ShardingRoute.DataSourceRoutes.Abstractions;

public abstract class AbstractFilterDataSourceRoute:AbstractDataSourceRoute
{
    protected AbstractFilterDataSourceRoute(IDataSourceRouteRule dataSourceRouteRule,TableMetadata tableMetadata) : base(dataSourceRouteRule,tableMetadata)
    {
    }
    public override ICollection<string> Route(SqlParserResult sqlParserResult)
    {
        var dataSourceNames = GetTableMetadata().DataSources;
        var dataSourceRouteRule = GetRouteRule();
        var beforeDataSources = dataSourceRouteRule.BeforeFilterDataSource(dataSourceNames);
        var routeDataSource = Route0(beforeDataSources,sqlParserResult);
        return dataSourceRouteRule.AfterFilterDataSource(dataSourceNames, beforeDataSources, routeDataSource);
    }

    protected abstract ICollection<string> Route0(ICollection<string> beforeDataSources,SqlParserResult sqlParserResult);

   

}