using ShardingConnector.ProxyServer.Binaries;

namespace ShardingConnector.ProxyServer.Abstractions;

public interface IServerHandler:IDisposable
{
    IServerResult Execute();
        
    bool Read()
    {
        return false;
    }

    BinaryRow GetRowData()
    {
        return new BinaryRow(new List<BinaryCell>(0));
    }

    void IDisposable.Dispose()
    {
        
    }
}