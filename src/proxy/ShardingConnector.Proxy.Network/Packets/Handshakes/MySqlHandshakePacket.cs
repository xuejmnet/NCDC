using System.Text;

namespace ShardingConnector.Proxy.Network.Packets.Handshakes;

public class MySqlHandshakePacket:MysqlPacket
{
    private readonly int protocolVersion = MySQLServerInfo.PROTOCOL_VERSION;
    private readonly string _serverVersion;
    private readonly int _connectionId;
    private readonly MySqlAuthPluginData _authPluginData;
    private readonly int _capabilityFlagsLower;
    private readonly int _capabilityFlagsUpper;
    private readonly int _characterSet;
    private readonly MySQLStatusFlagEnum _statusFlag;
    private string _authPluginName;

    public MySqlHandshakePacket(int connectionId,MySqlAuthPluginData authPluginData)
    {
        _serverVersion = MySQLServerInfo.SERVER_VERSION;
        _connectionId = connectionId;
        _capabilityFlagsLower = MySqlCapabilityFlag.CalculateHandshakeCapabilityFlagsLower();
        _capabilityFlagsUpper = MySqlCapabilityFlag.CalculateHandshakeCapabilityFlagsUpper();
        _characterSet = MySQLServerInfo.CHARSET;
        _statusFlag = MySQLStatusFlagEnum.SERVER_STATUS_AUTOCOMMIT;
        _authPluginData = authPluginData;
        _authPluginName = "mysql_native_password";
    }

    public void Write(MySqlPacketPayload payload)
    {
        payload.WriteInt1(protocolVersion);
        payload.WriteStringNul(_serverVersion);
        payload.WriteInt4(_connectionId);
        payload.WriteStringNul(Encoding.Default.GetString(_authPluginData.Part1));
        payload.WriteInt2(_capabilityFlagsLower);
        payload.WriteInt1(_characterSet);
        payload.WriteInt2((int)_statusFlag);
        payload.WriteInt2(_capabilityFlagsUpper);
        payload.WriteInt1(IsClientPluginAuth()?_authPluginData.GetAuthPluginData().Length+1:0);
        payload.WriteReserved(10);
        WriteAuthPluginDataPart2(payload);
        WriteAuthPluginName(payload);
    }
    private void WriteAuthPluginDataPart2( MySqlPacketPayload payload) {
        if (IsClientSecureConnection()) {
            payload.WriteStringNul(Encoding.Default.GetString(_authPluginData.Part2));
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
    public int GetSequenceId()
    {
        return 0;
    }

    public void Write(IPacketPayload payload)
    {
        Write((MySqlPacketPayload)payload);
    }
}