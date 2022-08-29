using OpenConnector.ProxyServer.Binaries;

namespace OpenConnector.ProxyServer.Abstractions;

public interface IServerHandler:IDisposable
{
    IServerResult Execute();
        
    bool Read()
    {
        return false;
    }

    BinaryRow GetRowData()
    {
        return BinaryRow.Empty;
    }

    void IDisposable.Dispose()
    {
        
    }
}