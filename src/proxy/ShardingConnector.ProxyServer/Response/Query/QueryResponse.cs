using System.Data.Common;

namespace ShardingConnector.ProxyServer.Response.Query;

public sealed class QueryResponse:IServerResponse
{
    public List<DbColumn> DbColumns { get; }
    public QueryResponse(List<DbColumn> dbColumns)
    {
        DbColumns = dbColumns;
    }
}