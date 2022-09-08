using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.Sharding.Abstractions;
using OpenConnector.Sharding.Extensions;
using OpenConnector.Sharding.Helpers;
using OpenConnector.ShardingAdoNet;
using OpenConnector.Shardings;

namespace OpenConnector.Sharding.Routes.DataSourceRoutes;

public abstract class AbstractOperatorDataSourceRoute:AbstractFilterDataSourceRoute
{
    protected override ICollection<string> Route0(ICollection<string> dataSources, ISqlCommandContext<ISqlCommand> sqlCommandContext,ParameterContext parameterContext)
    {
        var routePredicateExpression = ShardingHelper.GetRoutePredicateExpression(sqlCommandContext,parameterContext,TableMetadata,GetRouteFilter,false);
        var filter = routePredicateExpression.GetRoutePredicate();
        return dataSources.Where(o => filter(o)).ToList();
    }

    protected virtual Func<string, bool> GetRouteFilter(IComparable shardingValue,
        ShardingOperatorEnum shardingOperator, string columnName)
    {
        if (TableMetadata.IsMainShardingDataSourceColumn(columnName))
        {
            return GetRouteToFilter(shardingValue, shardingOperator);
        }

        return GetExtraRouteFilter(shardingValue, shardingOperator, columnName);
    }
    public abstract Func<string, bool> GetRouteToFilter(IComparable shardingValue,
        ShardingOperatorEnum shardingOperator);

    public virtual Func<string, bool> GetExtraRouteFilter(IComparable shardingValue,
        ShardingOperatorEnum shardingOperator, string shardingPropertyName)
    {
        throw new NotImplementedException(shardingPropertyName);
    }
}