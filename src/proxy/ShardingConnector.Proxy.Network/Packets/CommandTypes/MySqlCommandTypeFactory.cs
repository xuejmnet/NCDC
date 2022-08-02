namespace ShardingConnector.Proxy.Network.Packets.CommandTypes;

public static class MySqlCommandTypeFactory
{
    private static readonly IDictionary<int, MySqlCommandTypeEnum> MYSQL_COMMAND_PACKET_TYPE_CACHE =
        new Dictionary<int, MySqlCommandTypeEnum>();
    static MySqlCommandTypeFactory()
    {
        var mySqlCommandTypeEnums = Enum.GetValues<MySqlCommandTypeEnum>();
        foreach (var mySqlCommandType in mySqlCommandTypeEnums)
        {
            MYSQL_COMMAND_PACKET_TYPE_CACHE.Add((int)mySqlCommandType,mySqlCommandType);
        }
    }
    
    public static MySqlCommandTypeEnum ValueOf(int value)
    {
        if (!MYSQL_COMMAND_PACKET_TYPE_CACHE.TryGetValue(value, out var result))
        {
            throw new ArgumentException($"cant find {value} in command packet type");
        }

        return result;
    }
}