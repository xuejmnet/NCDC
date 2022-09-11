using NCDC.Configuration;
using NCDC.Configuration.Connection.Abstractions;
using OpenConnector.ProxyServer.Session.Connection.Abstractions;

namespace NCDC.Sharding.Executors.Context;


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