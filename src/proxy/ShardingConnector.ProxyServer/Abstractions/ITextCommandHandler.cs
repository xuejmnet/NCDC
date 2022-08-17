using ShardingConnector.ProxyServer.Response.Data;
using ShardingConnector.ProxyServer.Response.Header;

namespace ShardingConnector.ProxyServer.Abstractions;

/// <summary>
/// 文本协议处理
/// </summary>
public interface ITextCommandHandler:IDisposable
{
    IResponseHeader Execute();

    bool Next()
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