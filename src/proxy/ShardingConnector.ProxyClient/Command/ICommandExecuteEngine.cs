using DotNetty.Transport.Channels;
using ShardingConnector.ProtocolCore.Packets;
using ShardingConnector.ProtocolCore.Packets.Executor;
using ShardingConnector.ProtocolCore.Payloads;
using ShardingConnector.ProxyServer.Connection;
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

    IDatabasePacket GetErrorPacket(Exception exception);
    IDatabasePacket? GetOtherPacket(ConnectionSession connectionSession);

    void WriteQueryData(IChannelHandlerContext context, ServerConnection serverConnection,
        IQueryCommandExecutor queryCommandExecutor, int headerPackagesCount);
}