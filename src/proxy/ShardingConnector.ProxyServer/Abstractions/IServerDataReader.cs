using ShardingConnector.ProxyServer.Binaries;
using ShardingConnector.ProxyServer.StreamMerges;

namespace ShardingConnector.ProxyServer.Abstractions;

public interface IServerDataReader:IDisposable
{
    IServerResult ExecuteDbDataReader(CancellationToken cancellationToken=new CancellationToken());

    bool Read();
    BinaryRow GetRowData();
}