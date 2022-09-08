namespace OpenConnector.Sharding.Routes;

public interface IRouteValue
{
    string ColumnName { get; }
    string TableName { get; }
}