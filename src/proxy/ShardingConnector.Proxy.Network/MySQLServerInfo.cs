namespace ShardingConnector.Proxy.Network;

public class MySQLServerInfo
{
    public const int PROTOCOL_VERSION= 0x0A;
    public const string SERVER_VERSION = "5.7.2.3-ShardingConnector";
    /// <summary>
    ///  0x21 is utf8_general_ci.
    /// </summary>
    public const int CHARSET = 0x21;
}