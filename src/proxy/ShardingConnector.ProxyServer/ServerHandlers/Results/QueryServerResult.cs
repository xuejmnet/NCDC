using System.Data.Common;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Commons;

namespace ShardingConnector.ProxyServer.ServerHandlers.Results;

public sealed class QueryServerResult:IServerResult
{

    public ResultTypeEnum ResultType => ResultTypeEnum.QUERY;
    public List<DbColumn> DbColumns { get; }

    public QueryServerResult(List<DbColumn> dbColumns)
    {
        DbColumns = dbColumns;
    }
}