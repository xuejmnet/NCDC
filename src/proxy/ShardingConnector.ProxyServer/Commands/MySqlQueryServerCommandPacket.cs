using ShardingConnector.Protocol.MySql.Packet.Command;
using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.ProtocolMysql.Packet.ServerCommand;

namespace ShardingConnector.ProxyServer.Commands;

/// <summary>
/// 查询命令
/// </summary>
public class MySqlQueryServerCommandPacket : AbstractMySqlServerCommandPacket
{
    public string Sql { get; }

    public MySqlQueryServerCommandPacket(string sql) : base(MySqlCommandTypeEnum.COM_QUERY)
    {
        Sql = sql;
    }

    public MySqlQueryServerCommandPacket(MySqlPacketPayload payload) : base(MySqlCommandTypeEnum.COM_QUERY)
    {
        Sql = payload.ReadStringEOF();
    }

    protected override void DoWrite(MySqlPacketPayload payload)
    {
        payload.WriteStringEOF(Sql);
    }
}