namespace ShardingConnector.Protocol.MySql.Constant;

public class MySqlCapabilityFlag
{
    public static int CalculateHandshakeCapabilityFlagsLower()
    {
        return CalculateHandshakeCapabilityFlags
        (MySqlCapabilityFlagEnum.CLIENT_LONG_PASSWORD, MySqlCapabilityFlagEnum.CLIENT_FOUND_ROWS, MySqlCapabilityFlagEnum.CLIENT_LONG_FLAG, MySqlCapabilityFlagEnum.CLIENT_CONNECT_WITH_DB, MySqlCapabilityFlagEnum.CLIENT_ODBC, MySqlCapabilityFlagEnum.CLIENT_IGNORE_SPACE,
            MySqlCapabilityFlagEnum.CLIENT_PROTOCOL_41, MySqlCapabilityFlagEnum.CLIENT_INTERACTIVE, MySqlCapabilityFlagEnum.CLIENT_IGNORE_SIGPIPE, MySqlCapabilityFlagEnum.CLIENT_TRANSACTIONS, MySqlCapabilityFlagEnum.CLIENT_SECURE_CONNECTION) & 0x0000ffff;
    }
    public static int CalculateHandshakeCapabilityFlagsUpper() {
        return CalculateHandshakeCapabilityFlags(MySqlCapabilityFlagEnum.CLIENT_PLUGIN_AUTH) >> 16;
    }
    public static int CalculateHandshakeCapabilityFlags(params MySqlCapabilityFlagEnum[] capabilities)
    {
        int result = 0;
        foreach (var mySqlCapabilityFlagEnum in capabilities)
        {
            result |= (int)mySqlCapabilityFlagEnum;
        }

        return result;
    }
}