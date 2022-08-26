using ShardingConnector.Executor.Constant;
using ShardingConnector.Executor.Context;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Session;
using ShardingConnector.ProxyServer.StreamMerges;

namespace ShardingConnector.ProxyServer.ServerDataReaders;

public class AbstractAdoServerDataReader:AbstractExecuteServerDataReader
{
    protected ConnectionSession ConnectionSession { get; }

    public AbstractAdoServerDataReader(StreamMergeContext streamMergeContext,ConnectionSession connectionSession) : base(streamMergeContext)
    {
        ConnectionSession = connectionSession;
    }

    protected override List<IServerDbConnection> GetServerDbConnections(ConnectionModeEnum connectionMode, string dataSourceName, int connectionSize)
    {
        return ConnectionSession.ServerConnection.GetConnections(connectionMode, dataSourceName, connectionSize);
    }

}