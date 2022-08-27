using ShardingConnector.Executor.Constant;
using ShardingConnector.Executor.Context;
using ShardingConnector.ProxyServer.Session.Connection.Abstractions;

namespace ShardingConnector.ProxyServer.StreamMerges;


public sealed class CommandExecuteUnit
{
    public ExecutionUnit ExecutionUnit { get; }
    
    public IServerDbCommand ServerDbCommand{ get; }
    
    public ConnectionModeEnum ConnectionMode{ get; }

    public CommandExecuteUnit(ExecutionUnit executionUnit, IServerDbCommand serverDbCommand, ConnectionModeEnum connectionMode)
    {
        ExecutionUnit = executionUnit;
        ServerDbCommand = serverDbCommand;
        ConnectionMode = connectionMode;
    }
}