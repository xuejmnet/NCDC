using ShardingConnector.Protocol.MySql.Packet.Command;
using ShardingConnector.Protocol.MySql.Payload;

namespace ShardingConnector.ProxyClientMySql.ClientDataReader.SetOption;

public class MySqlSetOptionClientCommand
{
    public MySqlCommandTypeEnum CommandType => MySqlCommandTypeEnum.COM_SET_OPTION;
    public const int MYSQL_OPTION_MULTI_STATEMENTS_ON = 0;
    public const int MYSQL_OPTION_MULTI_STATEMENTS_OFF = 1;
    public int Value { get; }
    public MySqlSetOptionClientCommand(MySqlPacketPayload payload)
    {
        Value = payload.ReadInt2();
    }
}