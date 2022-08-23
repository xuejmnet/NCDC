using Microsoft.Extensions.Logging;
using ShardingConnector.Logger;
using ShardingConnector.Protocol.MySql.Packet.Command;
using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClientMySql.ClientDataReader.NotSupported;
using ShardingConnector.ProxyClientMySql.ClientDataReader.Query;
using ShardingConnector.ProxyClientMySql.ClientDataReader.Quit;
using ShardingConnector.ProxyClientMySql.ClientDataReader.SetOption;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Commands;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.ClientDataReader;

public sealed class MySqlClientDataReaderFactory:IClientDataReaderFactory
{
    private static readonly ILogger<MySqlClientDataReaderFactory> _logger = InternalLoggerFactory.CreateLogger<MySqlClientDataReaderFactory>();
    private readonly IServerHandlerFactory _serverHandlerFactory;

    public MySqlClientDataReaderFactory(IServerHandlerFactory serverHandlerFactory)
    {
        _serverHandlerFactory = serverHandlerFactory;
    }
    public IClientDataReader Create(IPacketPayload payload, ConnectionSession connectionSession)
    {
        var mySqlPacketPayload = (MySqlPacketPayload)payload;
        var commandType = GetCommandType(mySqlPacketPayload);
        return CreateServerCommandExecutor(commandType, mySqlPacketPayload, connectionSession);
    }

    public IClientDataReader CreateServerCommandExecutor(MySqlCommandTypeEnum commandType, MySqlPacketPayload payload,ConnectionSession connectionSession)
    {
        _logger.LogDebug($"create server command executor,command type: {commandType}");
        switch (commandType)
        {
            case MySqlCommandTypeEnum.COM_QUERY:return new MySqlQueryClientDataReader(new (payload),
                connectionSession, _serverHandlerFactory);
            case MySqlCommandTypeEnum.COM_QUIT: return new MySqlQuitClientDataReader();
            case MySqlCommandTypeEnum.COM_SET_OPTION: return new MySqlSetOptionClientDataReader(new (payload),connectionSession);
            default: return new MySqlNotSupportedClientDataReader(commandType);
        }
    }

    
    private MySqlCommandTypeEnum GetCommandType(MySqlPacketPayload payload)
    {
        return MySqlCommandTypeProvider.GetMySqlCommandType(payload);
    }
}