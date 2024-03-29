using NCDC.Protocol.MySql.Payload;

namespace NCDC.Protocol.MySql.Packet.Command;

public enum MySqlCommandTypeEnum
{
    COM_SLEEP=0x00,
    COM_QUIT=0x01,
    COM_INIT_DB=0x02,
    COM_QUERY=0x03,
    COM_FIELD_LIST=0x04,
    COM_CREATE_DB=0x05,
    COM_DROP_DB=0x06,
    COM_REFRESH=0x07,
    COM_SHUTDOWN=0x08,
    COM_STATISTICS=0x09,
    COM_PROCESS_INFO=0x0a,
    COM_CONNECT=0x0b,
    COM_PROCESS_KILL=0x0c,
    COM_DEBUG=0x0d,
    COM_PING=0x0e,
    COM_TIME=0x0f,
    COM_DELAYED_INSERT=0x10,
    COM_CHANGE_USER=0x11,
    COM_BINLOG_DUMP=0x12,
    COM_TABLE_DUMP=0x13,
    COM_CONNECT_OUT=0x14,
    COM_REGISTER_SLAVE=0x15,
    COM_STMT_PREPARE=0x16,
    COM_STMT_EXECUTE=0x17,
    COM_STMT_SEND_LONG_DATA=0x18,
    COM_STMT_CLOSE=0x19,
    COM_STMT_RESET=0x1a,
    COM_SET_OPTION=0x1b,
    COM_STMT_FETCH=0x1c,
    COM_DAEMON=0x1d,
    COM_BINLOG_DUMP_GTID=0x1e,
    COM_RESET_CONNECTION=0x1f
}

public sealed class MySqlCommandTypeProvider
{
    public static bool IsMySqlCommandType(int value)
    {
        return value >= (int)MySqlCommandTypeEnum.COM_SLEEP && value <= (int)MySqlCommandTypeEnum.COM_RESET_CONNECTION;
    }

    public static MySqlCommandTypeEnum GetMySqlCommandType(int value)
    {
        if (!IsMySqlCommandType(value))
        {
            throw new ArgumentException($"cannot find '{value}' in command packet type");
        }

        return (MySqlCommandTypeEnum)value;
    }
    public static MySqlCommandTypeEnum GetMySqlCommandType(MySqlPacketPayload payload)
    {
        if (0 != payload.ReadInt1())
        {
            throw new ArgumentException($" sequence id of mysql command packet must be `0`.");
        }

        var commandType = payload.ReadInt1();
        return GetMySqlCommandType(commandType);
    }
}