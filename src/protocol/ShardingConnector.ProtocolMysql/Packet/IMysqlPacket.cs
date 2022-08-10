using ShardingConnector.ProtocolCore.Packets;
using ShardingConnector.ProtocolMysql.Payload;

namespace ShardingConnector.ProtocolMysql.Packet;

public interface IMysqlPacket:IDatabasePacket<MySqlPacketPayload>
{
    int SequenceId { get; }
}