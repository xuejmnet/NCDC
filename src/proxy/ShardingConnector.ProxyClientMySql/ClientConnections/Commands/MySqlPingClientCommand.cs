using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClientMySql.ClientConnections.DataReaders.Ping;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlPingClientCommand:IClientCommand<MySqlPacketPayload>
{
    private readonly ConnectionSession _connectionSession;

    public MySqlPingClientCommand(ConnectionSession connectionSession)
    {
        _connectionSession = connectionSession;
    }
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        return new MySqlPingClientDataReader(_connectionSession);
    }
}