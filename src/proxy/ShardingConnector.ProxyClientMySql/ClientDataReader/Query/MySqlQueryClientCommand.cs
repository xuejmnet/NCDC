using ShardingConnector.Protocol.MySql.Packet.Command;
using ShardingConnector.Protocol.MySql.Payload;

namespace ShardingConnector.ProxyClientMySql.ClientDataReader.Query;

public sealed class MySqlQueryClientCommand
{
    public MySqlCommandTypeEnum CommandType => MySqlCommandTypeEnum.COM_QUERY;
    public string Sql { get; }
    public MySqlQueryClientCommand(MySqlPacketPayload payload)
    {
        Sql=Sql = payload.ReadStringEOF();
    }
}