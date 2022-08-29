// using System.Collections.Immutable;
// using OpenConnector.ProtocolCore.Packets;
//
// namespace OpenConnector.ProtocolMysql.Packet.Command;
//
// public class MySqlCommandPacketType : ICommandPacketType
// {
//     public int Value { get; }
//
//     public MySqlCommandPacketType(int value)
//     {
//         Value = value;
//     }
//
//     private static readonly IDictionary<int, MySqlCommandPacketType> _mysqlCommandPacketTypeCache;
//
//     /// <summary>
//     /// https://dev.mysql.com/doc/internals/en/com-sleep.html
//     /// </summary>
//     public  static MySqlCommandPacketType COM_SLEEP { get; } = new MySqlCommandPacketType(0x00);
//     public static MySqlCommandPacketType COM_QUIT { get; } = new MySqlCommandPacketType(0x01);
//     public static MySqlCommandPacketType COM_INIT_DB { get; } = new MySqlCommandPacketType(0x02);
//     public static MySqlCommandPacketType COM_QUERY { get; } = new MySqlCommandPacketType(0x03);
//     public static MySqlCommandPacketType COM_FIELD_LIST { get; } = new MySqlCommandPacketType(0x04);
//     public static MySqlCommandPacketType COM_CREATE_DB { get; } = new MySqlCommandPacketType(0x05);
//     public static MySqlCommandPacketType COM_DROP_DB { get; } = new MySqlCommandPacketType(0x06);
//     public static MySqlCommandPacketType COM_REFRESH { get; } = new MySqlCommandPacketType(0x07);
//     public static MySqlCommandPacketType COM_SHUTDOWN { get; } = new MySqlCommandPacketType(0x08);
//     public static MySqlCommandPacketType COM_STATISTICS { get; } = new MySqlCommandPacketType(0x09);
//     public static MySqlCommandPacketType COM_PROCESS_INFO { get; } = new MySqlCommandPacketType(0x0a);
//     public static MySqlCommandPacketType COM_CONNECT { get; } = new MySqlCommandPacketType(0x0b);
//     public static MySqlCommandPacketType COM_PROCESS_KILL { get; } = new MySqlCommandPacketType(0x0c);
//     public static MySqlCommandPacketType COM_DEBUG { get; } = new MySqlCommandPacketType(0x0d);
//     public static MySqlCommandPacketType COM_PING { get; } = new MySqlCommandPacketType(0x0e);
//     public static MySqlCommandPacketType COM_TIME { get; } = new MySqlCommandPacketType(0x0f);
//     public static MySqlCommandPacketType COM_DELAYED_INSERT { get; } = new MySqlCommandPacketType(0x10);
//     public static MySqlCommandPacketType COM_CHANGE_USER { get; } = new MySqlCommandPacketType(0x11);
//     public static MySqlCommandPacketType COM_BINLOG_DUMP { get; } = new MySqlCommandPacketType(0x12);
//     public static MySqlCommandPacketType COM_TABLE_DUMP { get; } = new MySqlCommandPacketType(0x13);
//     public static MySqlCommandPacketType COM_CONNECT_OUT { get; } = new MySqlCommandPacketType(0x14);
//     public static MySqlCommandPacketType COM_REGISTER_SLAVE { get; } = new MySqlCommandPacketType(0x15);
//     public static MySqlCommandPacketType COM_STMT_PREPARE { get; } = new MySqlCommandPacketType(0x16);
//     public static MySqlCommandPacketType COM_STMT_EXECUTE { get; } = new MySqlCommandPacketType(0x17);
//     public static MySqlCommandPacketType COM_STMT_SEND_LONG_DATA { get; } = new MySqlCommandPacketType(0x18);
//     public static MySqlCommandPacketType COM_STMT_CLOSE { get; } = new MySqlCommandPacketType(0x19);
//     public static MySqlCommandPacketType COM_STMT_RESET { get; } = new MySqlCommandPacketType(0x1a);
//     public static MySqlCommandPacketType COM_SET_OPTION { get; } = new MySqlCommandPacketType(0x1b);
//     public static MySqlCommandPacketType COM_STMT_FETCH { get; } = new MySqlCommandPacketType(0x1c);
//     public static MySqlCommandPacketType COM_DAEMON { get; } = new MySqlCommandPacketType(0x1d);
//     public static MySqlCommandPacketType COM_BINLOG_DUMP_GTID { get; } = new MySqlCommandPacketType(0x1e);
//     public static MySqlCommandPacketType COM_RESET_CONNECTION { get; } = new MySqlCommandPacketType(0x1f);
//
//     static MySqlCommandPacketType()
//     {
//         var commandPacketTypes = new Dictionary<int, MySqlCommandPacketType>()
//         {
//             { COM_SLEEP.Value, COM_SLEEP },
//             { COM_QUIT.Value, COM_QUIT },
//             { COM_INIT_DB.Value, COM_INIT_DB },
//             { COM_QUERY.Value, COM_QUERY },
//             { COM_FIELD_LIST.Value, COM_FIELD_LIST },
//             { COM_CREATE_DB.Value, COM_CREATE_DB },
//             { COM_DROP_DB.Value, COM_DROP_DB },
//             { COM_REFRESH.Value, COM_REFRESH },
//             { COM_SHUTDOWN.Value, COM_SHUTDOWN },
//             { COM_STATISTICS.Value, COM_STATISTICS },
//             { COM_PROCESS_INFO.Value, COM_PROCESS_INFO },
//             { COM_CONNECT.Value, COM_CONNECT },
//             { COM_PROCESS_KILL.Value, COM_PROCESS_KILL },
//             { COM_DEBUG.Value, COM_DEBUG },
//             { COM_PING.Value, COM_PING },
//             { COM_TIME.Value, COM_TIME },
//             { COM_DELAYED_INSERT.Value, COM_DELAYED_INSERT },
//             { COM_CHANGE_USER.Value, COM_CHANGE_USER },
//             { COM_BINLOG_DUMP.Value, COM_BINLOG_DUMP },
//             { COM_TABLE_DUMP.Value, COM_TABLE_DUMP },
//             { COM_CONNECT_OUT.Value, COM_CONNECT_OUT },
//             { COM_REGISTER_SLAVE.Value, COM_REGISTER_SLAVE },
//             { COM_STMT_PREPARE.Value, COM_STMT_PREPARE },
//             { COM_STMT_EXECUTE.Value, COM_STMT_EXECUTE },
//             { COM_STMT_SEND_LONG_DATA.Value, COM_STMT_SEND_LONG_DATA },
//             { COM_STMT_CLOSE.Value, COM_STMT_CLOSE },
//             { COM_STMT_RESET.Value, COM_STMT_RESET },
//             { COM_SET_OPTION.Value, COM_SET_OPTION },
//             { COM_STMT_FETCH.Value, COM_STMT_FETCH },
//             { COM_DAEMON.Value, COM_DAEMON },
//             { COM_BINLOG_DUMP_GTID.Value, COM_BINLOG_DUMP_GTID },
//             { COM_RESET_CONNECTION.Value, COM_RESET_CONNECTION },
//         };
//         _mysqlCommandPacketTypeCache = ImmutableDictionary.CreateRange<int, MySqlCommandPacketType>(commandPacketTypes);
//     }
//
//     public static MySqlCommandPacketType FindById(int value)
//     {
//         if (!_mysqlCommandPacketTypeCache.TryGetValue(value, out var commandPacketType))
//         {
//             throw new ArgumentException($"cantnot find '{value}' in command packet type");
//         }
//
//         return commandPacketType;
//     }
// }