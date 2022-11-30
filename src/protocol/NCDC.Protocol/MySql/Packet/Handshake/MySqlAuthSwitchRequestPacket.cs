using System.Text;
using NCDC.Protocol.MySql.Payload;

namespace NCDC.Protocol.MySql.Packet.Handshake;

public sealed class MySqlAuthSwitchRequestPacket : IMysqlPacket
{
    public const int HEADER = 0xfe;

    public MySqlAuthSwitchRequestPacket(MySqlPacketPayload payload, string authPluginName, MySqlAuthPluginData authPluginData)
    {
        SequenceId = payload.ReadInt1();
        if (HEADER != payload.ReadInt1())
        {
            throw new ArgumentException("Header of MySQL auth switch request packet must be `0xfe`.");
        }
        AuthPluginName = payload.ReadStringNul();
        var strAuthPluginData = payload.ReadStringNul();

        AuthPluginData = new MySqlAuthPluginData(CopyOfRange(Encoding.UTF8.GetBytes(strAuthPluginData), 0, 8),
            CopyOfRange(Encoding.UTF8.GetBytes(strAuthPluginData), 8, 20));
    }

    public MySqlAuthSwitchRequestPacket(int sequenceId,string authPluginName, MySqlAuthPluginData authPluginData)
    {
        SequenceId = sequenceId;
        AuthPluginName = authPluginName;
        AuthPluginData = authPluginData;
    }

    private byte[] CopyOfRange(byte[] original, int from, int to)
    {
        var newLength = to-from;
        if (newLength < 0)
            throw new ArgumentException($"{from} > {to}");
        var copy = new byte[newLength];
        Array.Copy(original,from,copy,0,Math.Min(original.Length-from,newLength));
        return copy;
    }

    public void WriteTo(MySqlPacketPayload payload)
    {
        payload.WriteInt1(HEADER);
        payload.WriteStringNul(AuthPluginName);
        payload.WriteStringNul(Encoding.UTF8.GetString(AuthPluginData.GetAuthPluginData()));
    }

    public int SequenceId { get; }
    public string AuthPluginName { get; }
    public MySqlAuthPluginData AuthPluginData { get; }
}