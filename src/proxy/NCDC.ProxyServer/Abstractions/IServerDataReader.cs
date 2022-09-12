using NCDC.ProxyServer.Binaries;
using NCDC.ProxyServer.StreamMerges;

namespace NCDC.ProxyServer.Abstractions;

public interface IServerDataReader:IDisposable
{
    IServerResult ExecuteDbDataReader(CancellationToken cancellationToken=new CancellationToken());

    bool Read();
    BinaryRow GetRowData();
}