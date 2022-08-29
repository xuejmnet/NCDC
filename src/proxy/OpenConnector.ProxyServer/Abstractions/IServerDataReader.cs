using OpenConnector.ProxyServer.Binaries;
using OpenConnector.ProxyServer.StreamMerges;

namespace OpenConnector.ProxyServer.Abstractions;

public interface IServerDataReader:IDisposable
{
    IServerResult ExecuteDbDataReader(CancellationToken cancellationToken=new CancellationToken());

    bool Read();
    BinaryRow GetRowData();
}