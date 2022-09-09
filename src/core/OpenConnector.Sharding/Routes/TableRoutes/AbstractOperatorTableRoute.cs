using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.Exceptions;
using OpenConnector.Sharding.Extensions;
using OpenConnector.Sharding.Helpers;
using OpenConnector.Sharding.Routes.Abstractions;
using OpenConnector.Sharding.Routes.DataSourceRoutes;
using OpenConnector.ShardingAdoNet;
using OpenConnector.Shardings;

namespace OpenConnector.Sharding.Routes.TableRoutes;

public abstract class AbstractOperatorTableRoute:AbstractFilterTableRoute
{
    public AbstractOperatorTableRoute(ITableMetadataManager tableMetadataManager) : base(tableMetadataManager)
    {
    }


    protected override ICollection<TableRouteUnit> Route0(DataSourceRouteResult dataSourceRouteResult,ICollection<string> beforeTableNames, SqlParserResult sqlParserResult)
    {
        var routePredicateExpression = ShardingHelper.GetRoutePredicateExpression(sqlParserResult,GetTableMetadata(),GetRouteFilter,true);
        var filter = routePredicateExpression.GetRoutePredicate();
        return FilterTableNameWithDataSourceResult(dataSourceRouteResult,beforeTableNames)
            .Where(o=> filter(o))
            .Select(ParseRouteWithTableName).ToList();
    }

    protected virtual Func<string, bool> GetRouteFilter(IComparable shardingValue,
        ShardingOperatorEnum shardingOperator, string columnName)
    {
        if (GetTableMetadata().IsMainShardingTableColumn(columnName))
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

    protected virtual IEnumerable<string> FilterTableNameWithDataSourceResult(
        DataSourceRouteResult dataSourceRouteResult,ICollection<string> beforeTableNames)
    {
        return beforeTableNames.Where(tableName =>
            dataSourceRouteResult.IntersectDataSources.Contains(ParseDataSourceWithTableName(tableName)));
    }

    protected virtual string ParseDataSourceWithTableName(string tableName)
    {
        return tableName.Split(".")[0];
    }

    protected virtual TableRouteUnit ParseRouteWithTableName(string tableName)
    {
        if (!tableName.Contains("."))
        {
            throw new ShardingNotSupportedException($"not supported table name:{tableName}");
        }

        var tableInfos = tableName.Split(".");
        return new TableRouteUnit(tableInfos[0], TableName,tableInfos[1]);
    }
}