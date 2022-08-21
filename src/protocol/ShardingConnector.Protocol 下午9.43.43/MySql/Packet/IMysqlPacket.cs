using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.Protocol.Packets;

namespace ShardingConnector.Protocol.MySql.Packet;

public interface IMysqlPacket:IPacket<MySqlPacketPayload>
{
    int SequenceId { get; }
}