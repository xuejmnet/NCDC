using OpenConnector.Protocol.MySql.Payload;
using OpenConnector.Protocol.Packets;

namespace OpenConnector.Protocol.MySql.Packet;

public interface IMysqlPacket:IPacket<MySqlPacketPayload>
{
    int SequenceId { get; }
}