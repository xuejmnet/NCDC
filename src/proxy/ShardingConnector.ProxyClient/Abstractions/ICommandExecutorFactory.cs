using ShardingConnector.AdoNet.Executor.Abstractions;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClient.Abstractions;

public interface ICommandExecutorFactory
{
    ICommandExecutor Create(IPacketPayload payload,ConnectionSession connectionSession);
}