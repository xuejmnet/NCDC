using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.Sharding.Extensions;
using OpenConnector.Sharding.Helpers;
using OpenConnector.Sharding.Routes.Abstractions;
using OpenConnector.ShardingAdoNet;
using OpenConnector.Shardings;

namespace OpenConnector.Sharding.Routes.DataSourceRoutes;

public abstract class AbstractOperatorDataSourceRoute:AbstractFilterDataSourceRoute
{

    protected AbstractOperatorDataSourceRoute(ITableMetadataManager tableMetadataManager) : base(tableMetadataManager)
    {
    }
    protected override ICollection<string> Route0(ICollection<string> dataSources, SqlParserResult sqlParserResult)
    {
        var routePredicateExpression = ShardingHelper.GetRoutePredicateExpression(sqlParserResult,GetTableMetadata(),GetRouteFilter,false);
        var filter = routePredicateExpression.GetRoutePredicate();
        return dataSources.Where(o => filter(o)).ToList();
    }

    protected virtual Func<string, bool> GetRouteFilter(IComparable shardingValue,
        ShardingOperatorEnum shardingOperator, string columnName)
    {
        if (GetTableMetadata().IsMainShardingDataSourceColumn(columnName))
        {
            return GetRouteToFilter(shardingValue, shardingOperator);
        }

        return GetExtraRouteFilter(shardingValue, shardingOperator, columnName);
    }
    public abstract Func<string, bool> GetRouteToFilter(IComparable shardingValue,
        ShardingOperatorEnum shardingOperator);

    public virtual Func<string, bool> GetExtraRouteFilter(IComparable shardingValue,
        ShardingOperatorEnum shardingOperator, string columnName)
    {
        throw new NotImplementedException(columnName);
    }
}