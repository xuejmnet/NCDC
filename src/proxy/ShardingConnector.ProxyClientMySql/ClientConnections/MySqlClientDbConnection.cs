using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using ShardingConnector.Logger;
using ShardingConnector.Protocol.MySql.Constant;
using ShardingConnector.Protocol.MySql.Packet.Command;
using ShardingConnector.Protocol.MySql.Packet.Generic;
using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClientMySql.ClientConnections.Commands;
using ShardingConnector.ProxyClientMySql.Common;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Commons;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.ClientConnections;

public sealed class MySqlClientDbConnection : IClientDbConnection<MySqlPacketPayload>
{
    private static readonly ILogger<MySqlClientDbConnection> _logger =
        InternalLoggerFactory.CreateLogger<MySqlClientDbConnection>();
    private readonly IServerHandlerFactory _serverHandlerFactory;
    private readonly IServerDataReaderFactory _serverDataReaderFactory;

    public MySqlClientDbConnection(IServerHandlerFactory serverHandlerFactory,IServerDataReaderFactory serverDataReaderFactory)
    {
        _serverHandlerFactory = serverHandlerFactory;
        _serverDataReaderFactory = serverDataReaderFactory;
    }

    public IClientCommand<MySqlPacketPayload> CreateCommand(MySqlPacketPayload payload,ConnectionSession connectionSession)
    {
        var commandType = GetCommandType(payload);
        _logger.LogDebug($"create server command executor,command type: {commandType}");
        switch (commandType)
        {
            case MySqlCommandTypeEnum.COM_QUIT:
                return new MySqlQuitClientCommand();
            case MySqlCommandTypeEnum.COM_FIELD_LIST:
                return new MySqlFieldListClientCommand(payload,connectionSession,_serverDataReaderFactory);
            case MySqlCommandTypeEnum.COM_INIT_DB:
                return new MySqlInitDbClientCommand(payload, connectionSession);
            case MySqlCommandTypeEnum.COM_QUERY:
                return new MySqlQueryClientCommand(payload, connectionSession, _serverHandlerFactory);
            case MySqlCommandTypeEnum.COM_STMT_PREPARE:
                return new MySqlStmtPrepareClientCommand(payload,connectionSession);
            // case MySqlCommandTypeEnum.COM_STMT_EXECUTE:
            //     return new MySqlStmtPrepareClientCommand(payload,connectionSession);
            case MySqlCommandTypeEnum.COM_SET_OPTION:
                return new MySqlSetOptionClientCommand(payload,connectionSession);
            case MySqlCommandTypeEnum.COM_PING:
                return new MySqlPingClientCommand(connectionSession);
            default: return new MySqlNotSupportedClientCommand(commandType);
        }
    }

    public void WriteQueryData(IChannelHandlerContext context, ConnectionSession connectionSession,
        IClientQueryDataReader<MySqlPacketPayload> clientQueryDataReader, int headerPackagesCount)
    {
        if (ResultTypeEnum.QUERY != clientQueryDataReader.ResultType || !context.Channel.Active)
        {
            return;
        }

        int count = 0;
        int flushThreshold = 10;
        // int flushThreshold = ProxyContext.getInstance().getContextManager().getMetaDataContexts().getMetaData()
        //     .getProps().<Integer > getValue(ConfigurationPropertyKey.PROXY_FRONTEND_FLUSH_THRESHOLD);
        int currentSequenceId = 0;
        while (clientQueryDataReader.MoveNext())
        {
            count++;
            while (!context.Channel.IsWritable && context.Channel.Active)
            {
                context.Flush();
                //当网络消费端消费过慢或者流量过大导致netty不可写入时但是链接还是激活的就等待网络恢复
                connectionSession.WaitChannelIsWritable();
                // ((JDBCBackendConnection)backendConnection).getResourceLock().doAwait();
            }

            var queryRowPacket = clientQueryDataReader.GetRowPacket();
            context.WriteAsync(queryRowPacket);
            if (flushThreshold == count)
            {
                context.Flush();
                count = 0;
            }

            currentSequenceId++;
        }

        context.WriteAsync(new MySqlEofPacket(++currentSequenceId + headerPackagesCount,ServerStatusFlagCalculator.CalculateFor(connectionSession)));
    }

    public IPacket<MySqlPacketPayload> GetErrorPacket(Exception exception)
    {
        return new MySqlErrPacket(1, MySqlServerErrorCode.ER_BAD_DB_ERROR_ARG1, "123");
    }

    public IPacket<MySqlPacketPayload>? GetOtherPacket(ConnectionSession connectionSession)
    {
        return null;
    }

    private MySqlCommandTypeEnum GetCommandType(MySqlPacketPayload payload)
    {
        return MySqlCommandTypeProvider.GetMySqlCommandType(payload);
    }
}