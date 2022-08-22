using ShardingConnector.Protocol.Packets;

namespace ShardingConnector.ProxyClient;

public interface ICommandExecutor:IDisposable
{
    List<IPacket> Execute();

    void IDisposable.Dispose()
    {
        
    }

}