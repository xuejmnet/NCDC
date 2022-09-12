using NCDC.Basic.Connection.Abstractions;
using NCDC.Enums;
using NCDC.ProxyServer.Executors;

namespace NCDC.ProxyServer.ServerDataReaders;

public abstract class AbstractAdoServerDataReader:AbstractExecuteServerDataReader
{
    protected IConnectionSession ConnectionSession { get; }

    public AbstractAdoServerDataReader(ShardingExecutionContext shardingExecutionContext,IConnectionSession connectionSession) : base(shardingExecutionContext)
    {
        ConnectionSession = connectionSession;
    }

    protected override List<IServerDbConnection> GetServerDbConnections(ConnectionModeEnum connectionMode, string dataSourceName, int connectionSize)
    {
        return ConnectionSession.ServerConnection.GetConnections(connectionMode, dataSourceName, connectionSize);
    }

}