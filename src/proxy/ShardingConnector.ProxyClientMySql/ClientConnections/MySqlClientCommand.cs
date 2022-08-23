// using Microsoft.Extensions.Logging;
// using ShardingConnector.Logger;
// using ShardingConnector.Protocol.MySql.Packet.Command;
// using ShardingConnector.Protocol.MySql.Payload;
// using ShardingConnector.ProxyClient.Abstractions;
// using ShardingConnector.ProxyClientMySql.ClientDataReader.NotSupported;
// using ShardingConnector.ProxyClientMySql.ClientDataReader.Query;
// using ShardingConnector.ProxyClientMySql.ClientDataReader.Quit;
// using ShardingConnector.ProxyClientMySql.ClientDataReader.SetOption;
// using ShardingConnector.ProxyServer.Abstractions;
// using ShardingConnector.ProxyServer.Session;
//
// namespace ShardingConnector.ProxyClientMySql.ClientConnections;
//
// public sealed class MySqlClientCommand:IClientCommand
// {
//     private readonly MySqlPacketPayload _payload;
//     private readonly ConnectionSession _connectionSession;
//     private readonly IServerHandlerFactory _serverHandlerFactory;
//
//     private static readonly ILogger<MySqlClientCommand> _logger =
//         InternalLoggerFactory.CreateLogger<MySqlClientCommand>();
//
//     public MySqlClientCommand(MySqlPacketPayload payload, ConnectionSession connectionSession,IServerHandlerFactory serverHandlerFactory)
//     {
//         _payload = payload;
//         _connectionSession = connectionSession;
//         _serverHandlerFactory = serverHandlerFactory;
//     }
//     public IClientDataReader ExecuteReader()
//     {
//         var commandType = GetCommandType(_payload);
//         return CreateClientDataReader(commandType, _payload, _connectionSession);
//     }
//     
//     private MySqlCommandTypeEnum GetCommandType(MySqlPacketPayload payload)
//     {
//         return MySqlCommandTypeProvider.GetMySqlCommandType(payload);
//     }
//     
//     private IClientDataReader CreateClientDataReader(MySqlCommandTypeEnum commandType, MySqlPacketPayload payload,ConnectionSession connectionSession)
//     {
//         _logger.LogDebug($"create server command executor,command type: {commandType}");
//         switch (commandType)
//         {
//             case MySqlCommandTypeEnum.COM_QUERY:return new MySqlQueryClientDataReader(new (payload),
//                 connectionSession, _serverHandlerFactory);
//             case MySqlCommandTypeEnum.COM_QUIT: return new MySqlQuitClientDataReader();
//             case MySqlCommandTypeEnum.COM_SET_OPTION: return new MySqlSetOptionClientDataReader(new (payload),connectionSession);
//             default: return new MySqlNotSupportedClientDataReader(commandType);
//         }
//     }
//
// }