using DotNetty.Transport.Channels;
using ShardingConnector.ProtocolCore.Packets;
using ShardingConnector.ProtocolCore.Packets.Executor;
using ShardingConnector.ProtocolCore.Payloads;
using ShardingConnector.ProtocolMysql.Constant;
using ShardingConnector.ProtocolMysql.Packet.Command;
using ShardingConnector.ProtocolMysql.Packet.Generic;
using ShardingConnector.ProtocolMysql.Payload;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClient.Command;
using ShardingConnector.ProxyServer.Connection;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.Command;

public sealed class MySqlCommandExecuteEngine:ICommandExecuteEngine
{
    private readonly ICommandExecutorFactory _commandExecutorFactory;

    public MySqlCommandExecuteEngine(ICommandExecutorFactory commandExecutorFactory)
    {
        _commandExecutorFactory = commandExecutorFactory;
    }
    public ICommandExecutor GetCommandExecutor(IPacketPayload payload,
        ConnectionSession connectionSession)
    {
        return _commandExecutorFactory.Create(payload, connectionSession);
    }

    public IDatabasePacket GetErrorPacket(Exception exception)
    {
        return new MySqlErrPacket(1, MySqlServerErrorCode.ER_BAD_DB_ERROR_ARG1, "123");
    }

    public IDatabasePacket? GetOtherPacket(ConnectionSession connectionSession)
    {
        return null;
    }

    public void WriteQueryData(IChannelHandlerContext context, ServerConnection serverConnection,
        IQueryCommandExecutor queryCommandExecutor, int headerPackagesCount)
    {
        throw new NotImplementedException();
    }
}