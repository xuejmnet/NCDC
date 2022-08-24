using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.ProxyClient.Abstractions;

namespace ShardingConnector.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlFieldListClientCommand:IClientCommand<MySqlPacketPayload>
{
    private readonly string _table;
    private readonly string _filedWildcard;
    public MySqlFieldListClientCommand(MySqlPacketPayload payload)
    {
        _table = payload.ReadStringNul();
        _filedWildcard = payload.ReadStringEOF();
    }
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        throw new NotImplementedException();
    }
}