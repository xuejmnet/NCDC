using NCDC.Enums;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.Executors;

namespace NCDC.ProxyServer.ServerDataReaders;

public abstract class AbstractAdoServerDataReader:AbstractExecuteServerDataReader
{

    public AbstractAdoServerDataReader(ShardingExecutionContext shardingExecutionContext,IConnectionSession connectionSession) : base(shardingExecutionContext,connectionSession)
    {
    }

    protected override ValueTask<List<IServerDbConnection>> GetServerDbConnectionsAsync(ConnectionModeEnum connectionMode, string dataSourceName, int connectionSize)
    {
        return ConnectionSession.ServerConnection.GetConnections(connectionMode, dataSourceName, connectionSize);
    }

}