using DotNetty.Transport.Channels;
using ShardingConnector.Protocol.MySql.Constant;
using ShardingConnector.Protocol.MySql.Packet.Generic;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClient.Command;
using ShardingConnector.ProxyClientMySql.Common;
using ShardingConnector.ProxyServer.Commons;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql;

public sealed class MySqlCommandExecuteEngine:ICommandExecuteEngine
{
    private readonly IClientDataReaderFactory _clientDataReaderFactory;

    public MySqlCommandExecuteEngine(IClientDataReaderFactory clientDataReaderFactory)
    {
        _clientDataReaderFactory = clientDataReaderFactory;
    }

    public IClientDataReader GetClientDataReader(IPacketPayload payload,
        ConnectionSession connectionSession)
    {
        return _clientDataReaderFactory.Create(payload, connectionSession);
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
        IClientQueryDataReader clientQueryDataReader, int headerPackagesCount)
    {
        if (ResultTypeEnum.QUERY != clientQueryDataReader.ResultType || !context.Channel.Active)
        {
            return;
        }

        int count = 0;
        int flushThreshold = 1;
        // int flushThreshold = ProxyContext.getInstance().getContextManager().getMetaDataContexts().getMetaData()
        //     .getProps().<Integer > getValue(ConfigurationPropertyKey.PROXY_FRONTEND_FLUSH_THRESHOLD);
        int currentSequenceId = 0;
        while (clientQueryDataReader.MoveNext())
        {
            count++;
            while (!context.Channel.IsWritable && context.Channel.Active)
            {
                context.Flush();
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

        context.WriteAsync(new MySqlEofPacket(++currentSequenceId + headerPackagesCount,
            (MySqlStatusFlagEnum)ServerStatusFlagCalculator.CalculateFor(connectionSession)));
        
    }
}