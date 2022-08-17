namespace ShardingConnector.ProxyServer.Response.Data;

public class QueryResponseCell
{
    public int ClrType { get; }
    public object Data { get; }
    public QueryResponseCell(int clrType, object data)
    {
        ClrType = clrType;
        Data = data;
    }
    
}