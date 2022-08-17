// using ShardingConnector.ProtocolCore.Packets;
// using ShardingConnector.ProtocolMysql.Packet.Command;
// using ShardingConnector.ProtocolMysql.Packet.Command.Admin;
// using ShardingConnector.ProtocolMysql.Packet.ServerCommand.Query;
// using ShardingConnector.ProtocolMysql.Payload;
// using ShardingConnector.ProxyServer.Session;
//
// namespace ShardingConnector.ProxyClientMySql.Command;
//
// public sealed class MySqlCommandPacketFactory
// {
//     public static ICommandPacket NewInstance(MySqlCommandTypeEnum mySqlCommandTypeEnum, MySqlPacketPayload payload,
//         ConnectionSession connectionSession)
//     {
//         switch (mySqlCommandTypeEnum)
//         {
//             case MySqlCommandTypeEnum.COM_QUERY: return new MySqlQueryServerCommandPacket(payload);
//             //MySqlCommandPacketType.COM_SET_OPTION
//             case MySqlCommandTypeEnum.COM_SET_OPTION: return new MySqlServerComSetOptionPacket(payload);
//                 default: return new MySqlServerNotSupportCommandPacket(mySqlCommandTypeEnum);
//         }
//     }
// }