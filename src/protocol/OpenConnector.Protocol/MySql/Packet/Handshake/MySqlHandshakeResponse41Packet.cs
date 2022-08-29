using System.Text;
using OpenConnector.Protocol.MySql.Constant;
using OpenConnector.Protocol.MySql.Payload;
using OpenConnector.Protocol.Packets;

namespace OpenConnector.Protocol.MySql.Packet.Handshake;

public class MySqlHandshakeResponse41Packet:IMysqlPacket
{
    private readonly int _sequenceId;
    private readonly int _maxPacketSize;
    private readonly int _characterSet;
    public  string Username { get; }
    public byte[] AuthResponse { get; }
    public int CapabilityFlags{ get; }
    public string? Database{ get; set; }
    public string? AuthPluginName{ get; }

    public MySqlHandshakeResponse41Packet(MySqlPacketPayload payload)
    {
        _sequenceId = payload.ReadInt1();
        CapabilityFlags = payload.ReadInt4();
        
        _maxPacketSize = payload.ReadInt4();
        _characterSet = payload.ReadInt1();
        payload.SkipReserved(23);
        Username = payload.ReadStringNul();
        AuthResponse = ReadAuthResponse(payload);
        Database = ReadDatabase(payload);
        AuthPluginName = ReadAuthPluginName(payload);
    }
    private byte[] ReadAuthResponse(MySqlPacketPayload payload) {
        if (0 != (CapabilityFlags & (int)MySqlCapabilityFlagEnum.CLIENT_PLUGIN_AUTH_LENENC_CLIENT_DATA)) {
            return payload.ReadStringLenencByBytes();
        }
        if (0 != (CapabilityFlags & (int)MySqlCapabilityFlagEnum.CLIENT_SECURE_CONNECTION)) {
            int length = payload.ReadInt1();
            return payload.ReadStringFixByBytes(length);
        }
        return payload.ReadStringNulByBytes();
    }
    
    private string? ReadDatabase( MySqlPacketPayload payload) {
        return 0 != (CapabilityFlags & (int)MySqlCapabilityFlagEnum.CLIENT_CONNECT_WITH_DB) ? payload.ReadStringNul() : null;
    }
    private string? ReadAuthPluginName( MySqlPacketPayload payload) {
        return 0 != (CapabilityFlags & (int)MySqlCapabilityFlagEnum.CLIENT_PLUGIN_AUTH) ? payload.ReadStringNul() : null;
    }

    public void WriteTo(MySqlPacketPayload payload)
    {
       payload.WriteInt4(CapabilityFlags);
       payload.WriteInt4(_maxPacketSize);
       payload.WriteInt1(_characterSet);
       payload.WriteReserved(23);
       payload.WriteStringNul(Username);
       WriteAuthResponse(payload);
       WriteDatabase(payload);
       WriteAuthPluginName(payload);
    }
    private void WriteAuthResponse( MySqlPacketPayload payload) {
        if (0 != (CapabilityFlags & (int)MySqlCapabilityFlagEnum.CLIENT_PLUGIN_AUTH_LENENC_CLIENT_DATA)) {
            payload.WriteStringLenenc(Encoding.UTF8.GetString(AuthResponse));
        } else if (0 != (CapabilityFlags & (int)MySqlCapabilityFlagEnum.CLIENT_SECURE_CONNECTION)) {
            payload.WriteInt1(AuthResponse.Length);
            payload.WriteBytes(AuthResponse);
        } else {
            payload.WriteStringNul(Encoding.UTF8.GetString(AuthResponse));
        }
    }
    
    private void WriteDatabase(MySqlPacketPayload payload) {
        if (0 != (CapabilityFlags & (int)MySqlCapabilityFlagEnum.CLIENT_CONNECT_WITH_DB)) {
            payload.WriteStringNul(Database!);
        }
    }
    private void WriteAuthPluginName(MySqlPacketPayload payload) {
        if (0 != (CapabilityFlags & (int)MySqlCapabilityFlagEnum.CLIENT_PLUGIN_AUTH)) {
            payload.WriteStringNul(AuthPluginName!);
        }
    }


    public void WriteTo(IPacketPayload payload)
    {
       WriteTo((MySqlPacketPayload)payload);
    }

    public int SequenceId => _sequenceId;
    public int CharacterSet => _characterSet;
}