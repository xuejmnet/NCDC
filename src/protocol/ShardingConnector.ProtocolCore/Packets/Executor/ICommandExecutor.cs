namespace ShardingConnector.ProtocolCore.Packets.Executor;

public interface ICommandExecutor:IDisposable
{
    List<IDatabasePacket> Execute();

    void IDisposable.Dispose()
    {
        
    }

}