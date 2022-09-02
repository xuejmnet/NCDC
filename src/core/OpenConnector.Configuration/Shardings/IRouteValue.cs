namespace OpenConnector.Shardings;

public interface IRouteValue
{
    string TableName { get; }
    string ColumnName { get; }
    ShardingPredicateEnum ShardingPredicate { get; }
    List<IShardingValue> ShardingValues { get; }
}