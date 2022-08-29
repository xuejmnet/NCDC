using System.Collections.Concurrent;

namespace OpenConnector.Protocol.MySql.Constant;

public static class MySqlServerInfo
{
    public const int PROTOCOL_VERSION= 0x0A;
    public static readonly MySqlCharacterSet DEFAULT_CHARSET = MySqlCharacterSet.UTF8MB4_GENERAL_CI;
    private static string _defaultMySqlVersion= "5.7.22";
    public const string SERVER_VERSION_PATTERN = "{0}-OpenConnector-Proxy {1}";
    private static readonly IDictionary<string, string> SERVER_VERSIONS = new ConcurrentDictionary<string, string>();

    public static string GetDefaultServerVersion()
    {
        return string.Format(SERVER_VERSION_PATTERN, _defaultMySqlVersion, "0.0.1");
    }
}