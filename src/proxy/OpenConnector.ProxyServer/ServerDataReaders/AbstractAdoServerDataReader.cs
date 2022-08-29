using OpenConnector.ProxyServer.Abstractions;
using OpenConnector.ProxyServer.Commons;
using OpenConnector.ProxyServer.Session;
using OpenConnector.ProxyServer.Session.Connection.Abstractions;
using OpenConnector.ProxyServer.StreamMerges.Executors.Context;

namespace OpenConnector.ProxyServer.ServerDataReaders;

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