using DotNetty.Transport.Channels;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClient.Command;

public interface ICommandExecuteEngine
{
    IClientDataReader GetClientDataReader(IPacketPayload payload,
        ConnectionSession connectionSession);

    IPacket GetErrorPacket(Exception exception);
    IPacket? GetOtherPacket(ConnectionSession connectionSession);

    void WriteQueryData(IChannelHandlerContext context, ConnectionSession connectionSession,
        IClientQueryDataReader clientQueryDataReader, int headerPackagesCount);
}