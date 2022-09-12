// using NCDC.Exceptions;
// using NCDC.ProtocolMysql.Payload;
//
// namespace NCDC.ProtocolMysql.Packet.Command;
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