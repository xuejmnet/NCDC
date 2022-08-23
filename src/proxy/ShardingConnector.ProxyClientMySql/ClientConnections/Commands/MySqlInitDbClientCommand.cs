using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.ProxyClient.Abstractions;

namespace ShardingConnector.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlInitDbClientCommand:IClientCommand<MySqlPacketPayload>
{
    private readonly string schema;
    public MySqlInitDbClientCommand(MySqlPacketPayload payload)
    {
        schema = payload.ReadStringEOF();
    }
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        throw new NotImplementedException();
    }
}