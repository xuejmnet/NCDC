using ShardingConnector.ProtocolCore.Packets;
using ShardingConnector.ProtocolMysql.Payloads;

namespace ShardingConnector.ProtocolMysql.Packets;

public interface IMysqlPacket:IDatabasePacket<MySqlPacketPayload>
{
    int SequenceId { get; }
}