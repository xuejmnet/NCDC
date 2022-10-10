using NCDC.ProxyServer.Binaries;

namespace NCDC.ProxyServer.Abstractions;

public interface IServerHandler:IDisposable
{
    Task<IServerResult> ExecuteAsync();
        
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