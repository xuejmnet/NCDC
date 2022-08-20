using ShardingConnector.ProxyServer.Response;
using ShardingConnector.ProxyServer.Response.Data;

namespace ShardingConnector.ProxyServer.Abstractions;

/// <summary>
/// 文本协议处理
/// </summary>
public interface ITextCommandHandler:IDisposable
{
    IServerResponse Execute();

    bool MoveNext()
    {
        return false;
    }

    QueryResponseRow GetRowData()
    {
        return new QueryResponseRow(new List<QueryResponseCell>(0));
    }

    void IDisposable.Dispose()
    {
        
    }
}