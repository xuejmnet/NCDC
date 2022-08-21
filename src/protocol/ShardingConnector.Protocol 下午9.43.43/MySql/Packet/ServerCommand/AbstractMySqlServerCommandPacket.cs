// using ShardingConnector.Protocol.MySql.Packet;
// using ShardingConnector.Protocol.MySql.Packet.Command;
// using ShardingConnector.Protocol.MySql.Payload;
// using ShardingConnector.ProtocolCore.Packets;
// using ShardingConnector.ProtocolCore.Payloads;
// using ShardingConnector.ProtocolMysql.Packet.Command;
//
// namespace ShardingConnector.ProtocolMysql.Packet.ServerCommand;
//
// /// <summary>
// /// mysql 服务端发送的命令包抽象
// /// </summary>
// public abstract class AbstractMySqlServerCommandPacket:IMysqlPacket,IServerCommandPacket
// {
//     private readonly MySqlCommandTypeEnum _commandType;
//
//     public AbstractMySqlServerCommandPacket(MySqlCommandTypeEnum commandType)
//     {
//         SequenceId = 0;
//         _commandType = commandType;
//     }
//     public void Write(MySqlPacketPayload payload)
//     {
//         payload.WriteInt1((int)_commandType);
//         DoWrite(payload);
//     }
//
//     protected virtual void DoWrite(MySqlPacketPayload payload)
//     {
//         
//     }
//
//     public void Write(IPacketPayload payload)
//     {
//         Write((MySqlPacketPayload)payload);
//     }
//
//     public int SequenceId { get; }
// }