using DotNetty.Transport.Channels;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClient.Command;

public interface ICommandExecuteEngine
{
    // ICommandPacketType GetCommandPacketType(IPacketPayload payload);
    //
    // ICommandPacket GetCommandPacket(IPacketPayload payload, ICommandPacketType commandPacketType,
    //     ConnectionSession connectionSession);

    ICommandExecutor GetCommandExecutor(IPacketPayload payload,
        ConnectionSession connectionSession);

    IPacket GetErrorPacket(Exception exception);
    IPacket? GetOtherPacket(ConnectionSession connectionSession);

    void WriteQueryData(IChannelHandlerContext context, ConnectionSession connectionSession,
        IQueryCommandExecutor queryCommandExecutor, int headerPackagesCount);
}