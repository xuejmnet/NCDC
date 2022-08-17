using ShardingConnector.ProtocolCore.Packets.Executor;
using ShardingConnector.ProtocolCore.Payloads;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClient.Abstractions;

public interface ICommandExecutorFactory
{
    ICommandExecutor Create(IPacketPayload payload,ConnectionSession connectionSession);
}