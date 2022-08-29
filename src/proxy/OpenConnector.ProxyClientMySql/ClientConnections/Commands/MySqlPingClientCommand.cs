using OpenConnector.Protocol.MySql.Payload;
using OpenConnector.ProxyClient.Abstractions;
using OpenConnector.ProxyClientMySql.ClientConnections.DataReaders.Ping;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyClientMySql.ClientConnections.Commands;

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