using ShardingConnector.Executor.Constant;
using ShardingConnector.Executor.Context;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Session;
using ShardingConnector.ProxyServer.Session.Connection.Abstractions;

namespace ShardingConnector.ProxyServer.ServerDataReaders;

public abstract class AbstractAdoServerDataReader:AbstractExecuteServerDataReader
{
    protected ConnectionSession ConnectionSession { get; }

    public AbstractAdoServerDataReader(ShardingExecutionContext shardingExecutionContext,ConnectionSession connectionSession) : base(shardingExecutionContext)
    {
        ConnectionSession = connectionSession;
    }

    protected override List<IServerDbConnection> GetServerDbConnections(ConnectionModeEnum connectionMode, string dataSourceName, int connectionSize)
    {
        return ConnectionSession.ServerConnection.GetConnections(connectionMode, dataSourceName, connectionSize);
    }

}