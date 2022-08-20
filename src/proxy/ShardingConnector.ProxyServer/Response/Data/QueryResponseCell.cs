namespace ShardingConnector.ProxyServer.Response.Data;

public class QueryResponseCell
{
    public Type ClrType { get; }
    public object? Data { get; }
    public QueryResponseCell(Type clrType, object? data)
    {
        ClrType = clrType;
        Data = data;
    }
    
}