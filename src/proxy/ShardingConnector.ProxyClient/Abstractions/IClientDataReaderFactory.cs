using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClient.Abstractions;

public interface IClientDataReaderFactory
{
    IClientDataReader Create(IPacketPayload payload, ConnectionSession connectionSession);
}