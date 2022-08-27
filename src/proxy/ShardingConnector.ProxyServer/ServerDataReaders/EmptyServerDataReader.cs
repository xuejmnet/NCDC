using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Binaries;
using ShardingConnector.ProxyServer.ServerHandlers.Results;
using ShardingConnector.ProxyServer.StreamMerges;
using ShardingConnector.ProxyServer.StreamMerges.Results;

namespace ShardingConnector.ProxyServer.ServerDataReaders;

public sealed class EmptyServerDataReader:IServerDataReader
{
    private EmptyServerDataReader(){}
    public static EmptyServerDataReader Instance { get; } = new EmptyServerDataReader();
    public IServerResult ExecuteDbDataReader(CancellationToken cancellationToken = new CancellationToken())
    {
        return new RecordsAffectedServerResult();
    }

    public bool Read()
    {
        return false;
    }

    public BinaryRow GetRowData()
    {
        return BinaryRow.Empty;
    }

    public void Dispose()
    {
    }
}