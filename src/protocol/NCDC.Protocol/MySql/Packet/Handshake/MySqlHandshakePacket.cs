using System.Text;
using NCDC.Protocol.MySql.Constant;
using NCDC.Protocol.MySql.Constant.CharacterSets;
using NCDC.Protocol.MySql.Payload;
using NCDC.Protocol.Packets;

namespace NCDC.Protocol.MySql.Packet.Handshake;

/// <summary>
/// https://dev.mysql.com/doc/internals/en/connection-phase-packets.html#packet-Protocol::Handshake
/// </summary>
public sealed class MySqlHandshakePacket:IMysqlPacket
{
    private readonly int protocolVersion = MySqlServerInfo.PROTOCOL_VERSION;
    private readonly string _serverVersion;
    private readonly int _connectionId;
    private readonly MySqlAuthPluginData _authPluginData;
    private readonly int _capabilityFlagsLower;
    private readonly int _capabilityFlagsUpper;
    private readonly MySqlCharacterSetEnum _characterSet;
    private readonly MySqlStatusFlagEnum _statusFlag;
    private string _authPluginName;

    public MySqlHandshakePacket(int connectionId,MySqlAuthPluginData authPluginData)
    {
        _serverVersion = MySqlServerInfo.GetDefaultServerVersion();
        _connectionId = connectionId;
        _capabilityFlagsLower = MySqlCapabilityFlag.CalculateHandshakeCapabilityFlagsLower();
        _capabilityFlagsUpper = MySqlCapabilityFlag.CalculateHandshakeCapabilityFlagsUpper();
        _characterSet = MySqlServerInfo.DEFAULT_CHARSET.Value;
        _statusFlag = MySqlStatusFlagEnum.SERVER_STATUS_AUTOCOMMIT;
        _authPluginData = authPluginData;
        _authPluginName = MySqlAuthenticationMethod.NATIVE_PASSWORD_AUTHENTICATION;
    }

    public void WriteTo(MySqlPacketPayload payload)
    {
        payload.WriteInt1(protocolVersion);
        payload.WriteStringNul(_serverVersion);
        payload.WriteInt4(_connectionId);
        payload.WriteStringNul(Encoding.UTF8.GetString(_authPluginData.Part1));
        payload.WriteInt2(_capabilityFlagsLower);
        payload.WriteInt1((int)_characterSet);
        payload.WriteInt2((int)_statusFlag);
        payload.WriteInt2(_capabilityFlagsUpper);
        payload.WriteInt1(IsClientPluginAuth()?_authPluginData.GetAuthPluginData().Length+1:0);
        payload.WriteReserved(10);
        WriteAuthPluginDataPart2(payload);
        WriteAuthPluginName(payload);
    }
    private void WriteAuthPluginDataPart2( MySqlPacketPayload payload) {
        if (IsClientSecureConnection()) {
            payload.WriteStringNul(Encoding.UTF8.GetString(_authPluginData.Part2));
        }
    }
    private void WriteAuthPluginName( MySqlPacketPayload payload) {
        if (IsClientPluginAuth()) {
            payload.WriteStringNul(_authPluginName);
        }
    }

    private bool IsClientSecureConnection()
    {
        return 0 != (_capabilityFlagsLower & (int)MySqlCapabilityFlagEnum.CLIENT_SECURE_CONNECTION & 0x00000ffff);
    }
    private bool IsClientPluginAuth() {
        return 0 != (_capabilityFlagsUpper & (int)MySqlCapabilityFlagEnum.CLIENT_PLUGIN_AUTH >> 16);
    }

    public int SequenceId => 0;

    public void WriteTo(IPacketPayload payload)
    {
        WriteTo((MySqlPacketPayload)payload);
    }

}