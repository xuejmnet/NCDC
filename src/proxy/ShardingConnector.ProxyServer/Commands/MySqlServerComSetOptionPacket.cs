using ShardingConnector.Protocol.MySql.Packet.Command;
using ShardingConnector.Protocol.MySql.Payload;

namespace ShardingConnector.ProxyServer.Commands;

public class MySqlServerComSetOptionPacket:AbstractMySqlServerCommandPacket
{
    public const int MYSQL_OPTION_MULTI_STATEMENTS_ON = 0;
    public const int MYSQL_OPTION_MULTI_STATEMENTS_OFF = 1;
    public int Value { get; }
    public MySqlServerComSetOptionPacket(MySqlPacketPayload payload) : base(MySqlCommandTypeEnum.COM_SET_OPTION)
    {
        Value = payload.ReadInt2();
    }
}