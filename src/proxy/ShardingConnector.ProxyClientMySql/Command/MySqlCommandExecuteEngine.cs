using DotNetty.Transport.Channels;
using ShardingConnector.Protocol.MySql.Constant;
using ShardingConnector.Protocol.MySql.Packet.Generic;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClient.Command;
using ShardingConnector.ProxyServer;
using ShardingConnector.ProxyServer.Connection;
using ShardingConnector.ProxyServer.Session;
using ICommandExecutor = ShardingConnector.ProxyClient.ICommandExecutor;
using IQueryCommandExecutor = ShardingConnector.ProxyClient.IQueryCommandExecutor;

namespace ShardingConnector.ProxyClientMySql.Command;

public sealed class MySqlCommandExecuteEngine : ICommandExecuteEngine
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

    public IPacket GetErrorPacket(Exception exception)
    {
        return new MySqlErrPacket(1, MySqlServerErrorCode.ER_BAD_DB_ERROR_ARG1, "123");
    }

    public IPacket? GetOtherPacket(ConnectionSession connectionSession)
    {
        return null;
    }

    public void WriteQueryData(IChannelHandlerContext context, ConnectionSession connectionSession,
        IQueryCommandExecutor queryCommandExecutor, int headerPackagesCount)
    {
        if (ResponseTypeEnum.QUERY != queryCommandExecutor.GetResponseType() || !context.Channel.Active)
        {
            return;
        }

        int count = 0;
        int flushThreshold = 1;
        // int flushThreshold = ProxyContext.getInstance().getContextManager().getMetaDataContexts().getMetaData()
        //     .getProps().<Integer > getValue(ConfigurationPropertyKey.PROXY_FRONTEND_FLUSH_THRESHOLD);
        int currentSequenceId = 0;
        while (queryCommandExecutor.MoveNext())
        {
            count++;
            while (!context.Channel.IsWritable && context.Channel.Active)
            {
                context.Flush();
                // ((JDBCBackendConnection)backendConnection).getResourceLock().doAwait();
            }

            var queryRowPacket = queryCommandExecutor.GetQueryRowPacket();
            context.WriteAsync(queryRowPacket);
            if (flushThreshold == count)
            {
                context.Flush();
                count = 0;
            }

            currentSequenceId++;
        }

        context.WriteAsync(new MySqlEofPacket(++currentSequenceId + headerPackagesCount,
            (MySqlStatusFlagEnum)ServerStatusFlagCalculator.CalculateFor(connectionSession)));
        
    }
}