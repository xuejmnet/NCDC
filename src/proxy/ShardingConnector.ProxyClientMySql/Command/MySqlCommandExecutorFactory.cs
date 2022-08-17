// using Microsoft.Extensions.Logging;
// using ShardingConnector.Logger;
// using ShardingConnector.ProtocolCore.Packets;
// using ShardingConnector.ProtocolCore.Packets.Executor;
// using ShardingConnector.ProtocolMysql.Packet.Command;
// using ShardingConnector.ProtocolMysql.Packet.Command.Admin;
// using ShardingConnector.ProxyClientMySql.Command.Admin;
// using ShardingConnector.ProxyClientMySql.Command.Generic;
// using ShardingConnector.ProxyServer.Session;
//
// namespace ShardingConnector.ProxyClientMySql.Command;
//
// public sealed class MySqlCommandExecutorFactory
// {
//     private static readonly ILogger<MySqlCommandExecutorFactory> _logger =
//         InternalLoggerFactory.CreateLogger<MySqlCommandExecutorFactory>();
//     public static ICommandExecutor NewInstance(MySqlCommandTypeEnum commandPacketTypeEnum, ICommandPacket commandPacket,
//         ConnectionSession connectionSession)
//     {
//         _logger.LogDebug($"execute packet type :{commandPacketTypeEnum},value: {commandPacket}");
//         switch (commandPacketTypeEnum)
//         {
//             case  MySqlCommandTypeEnum.COM_QUERY: return new MySqlComSetOptionExecutor((MySqlServerComSetOptionPacket)commandPacket,connectionSession);
//             case  MySqlCommandTypeEnum.COM_SET_OPTION: return new MySqlComSetOptionExecutor((MySqlServerComSetOptionPacket)commandPacket,connectionSession);
//             default: return new MySqlNotSupportedCommandExecutor(commandPacketTypeEnum);
//         }
//     }
// }