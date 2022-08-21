// using ShardingConnector.Exceptions;
// using ShardingConnector.ProtocolMysql.Payload;
//
// namespace ShardingConnector.ProtocolMysql.Packet.Command;
//
// public sealed class MySqlCommandPacketTypeLoader
// {
//     public static MySqlCommandPacketType GetCommandPacketType(MySqlPacketPayload payload)
//     {
//         if (0 != payload.ReadInt1())
//         {
//             throw new ArgumentException($" sequence id of mysql command packet must be `0`.");
//         }
//
//         return MySqlCommandPacketType.FindById(payload.ReadInt1());
//     }
// }