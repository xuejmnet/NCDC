using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClientMySql.ClientConnections.DataReaders.Quit;

namespace ShardingConnector.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlQuitClientCommand:IClientCommand<MySqlPacketPayload>
{
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        return new MySqlQuitClientDataReader();
    }
}