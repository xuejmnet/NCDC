using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.Sharding.Abstractions;

public abstract class AbstractFilterDataSourceRoute:AbstractDataSourceRoute
{
    public override ICollection<string> Route(ISqlCommandContext<ISqlCommand> sqlCommandContext,ParameterContext parameterContext)
    {
        var dataSourceNames = GetDataSourceNames();
        var beforeDataSources = BeforeFilterDataSource(dataSourceNames);
        var routeDataSource = Route0(beforeDataSources,sqlCommandContext,parameterContext);
        return AfterFilterDataSource(dataSourceNames, beforeDataSources, routeDataSource);
    }

    protected abstract ICollection<string> Route0(ICollection<string> beforeDataSources,ISqlCommandContext<ISqlCommand> sqlCommandContext,ParameterContext parameterContext);

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