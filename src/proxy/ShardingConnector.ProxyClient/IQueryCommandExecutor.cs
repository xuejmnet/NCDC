
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyServer;

namespace ShardingConnector.ProxyClient;

public interface IQueryCommandExecutor:ICommandExecutor
{
    ResponseTypeEnum GetResponseType();

    bool MoveNext()
    {
        return false;
    }
    IPacket GetQueryRowPacket();
}