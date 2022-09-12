using NCDC.Protocol.MySql.Payload;
using NCDC.Protocol.Packets;

namespace NCDC.Protocol.MySql.Packet;

public interface IMysqlPacket:IPacket<MySqlPacketPayload>
{
    int SequenceId { get; }
}