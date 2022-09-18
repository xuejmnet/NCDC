using NCDC.Protocol.MySql.Payload;

namespace NCDC.Protocol.MySql.Packet.Handshake;

public sealed class MySqlAuthSwitchResponsePacket : IMysqlPacket
{
    public MySqlAuthSwitchResponsePacket(MySqlPacketPayload payload)
    {
        SequenceId = payload.ReadInt1();
        AuthPluginResponse = payload.ReadStringEOFByBytes();
    }
    public void WriteTo(MySqlPacketPayload payload)
    {
        payload.WriteBytes(AuthPluginResponse);
    }

    public int SequenceId { get; }
    public byte[] AuthPluginResponse { get; }
}