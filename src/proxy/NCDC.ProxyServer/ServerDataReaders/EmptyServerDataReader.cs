using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Binaries;
using NCDC.ProxyServer.ServerHandlers.Results;
using NCDC.ProxyServer.StreamMerges;
using NCDC.ProxyServer.StreamMerges.Results;

namespace NCDC.ProxyServer.ServerDataReaders;

public sealed class EmptyServerDataReader:IServerDataReader
{
    private EmptyServerDataReader(){}
    public static EmptyServerDataReader Instance { get; } = new EmptyServerDataReader();
    public Task<IServerResult> ExecuteDbDataReaderAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult((IServerResult)new RecordsAffectedServerResult());
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