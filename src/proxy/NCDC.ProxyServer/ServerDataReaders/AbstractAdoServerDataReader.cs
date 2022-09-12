using OpenConnector.Configuration;
using OpenConnector.Configuration.Session;
using NCDC.ProxyServer.Session.Connection.Abstractions;
using NCDC.ProxyServer.StreamMerges.Executors.Context;

namespace NCDC.ProxyServer.ServerDataReaders;

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