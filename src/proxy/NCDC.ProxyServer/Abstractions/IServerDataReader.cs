using NCDC.ProxyServer.Binaries;
using NCDC.ProxyServer.StreamMerges;

namespace NCDC.ProxyServer.Abstractions;

public interface IServerDataReader:IDisposable
{
    Task<IServerResult> ExecuteDbDataReaderAsync(CancellationToken cancellationToken=new CancellationToken());

    bool Read();
    BinaryRow GetRowData();
}