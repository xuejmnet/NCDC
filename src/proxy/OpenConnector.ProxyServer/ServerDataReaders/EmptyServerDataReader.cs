using OpenConnector.ProxyServer.Abstractions;
using OpenConnector.ProxyServer.Binaries;
using OpenConnector.ProxyServer.ServerHandlers.Results;
using OpenConnector.ProxyServer.StreamMerges;
using OpenConnector.ProxyServer.StreamMerges.Results;

namespace OpenConnector.ProxyServer.ServerDataReaders;

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