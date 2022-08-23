using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClientMySql.ClientDataReader.Quit;

namespace ShardingConnector.ProxyClientMySql.ClientConnections.Commands;

public class MySqlQuitClientCommand:IClientCommand<MySqlPacketPayload>
{
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        return new MySqlQuitClientDataReader();
    }
}