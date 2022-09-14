using NCDC.Extensions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.Executors;

namespace NCDC.ProxyServer.ServerDataReaders;

public sealed class ServerDataReaderFactory:IServerDataReaderFactory
{
    private readonly IShardingExecutionContextFactory _shardingExecutionContextFactory;

    public ServerDataReaderFactory(IShardingExecutionContextFactory shardingExecutionContextFactory)
    {
        _shardingExecutionContextFactory = shardingExecutionContextFactory;
    }
    public IServerDataReader Create(string sql, IConnectionSession connectionSession)
    {
        var shardingExecutionContext =_shardingExecutionContextFactory.Create(sql);
        if (shardingExecutionContext.GetExecutionUnits().IsEmpty())
        {
            return EmptyServerDataReader.Instance;
        }
        return new QueryServerDataReader(shardingExecutionContext, connectionSession);
    }
}