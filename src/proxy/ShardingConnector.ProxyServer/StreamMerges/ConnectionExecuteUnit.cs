using ShardingConnector.Executor.Constant;
using ShardingConnector.Executor.Context;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Session.Connection.Abstractions;

namespace ShardingConnector.ProxyServer.StreamMerges;

public class ConnectionExecuteUnit
{
    
    public ExecutionUnit ExecutionUnit { get; }
    
    public IServerDbConnection Connection{ get; }
    
    public ConnectionModeEnum ConnectionMode{ get; }

    public ConnectionExecuteUnit(ExecutionUnit executionUnit, IServerDbConnection connection, ConnectionModeEnum connectionMode)
    {
        ExecutionUnit = executionUnit;
        Connection = connection;
        ConnectionMode = connectionMode;
    }
}