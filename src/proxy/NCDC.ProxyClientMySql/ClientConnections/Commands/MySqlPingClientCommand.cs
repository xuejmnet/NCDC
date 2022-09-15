using NCDC.Protocol.MySql.Payload;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClientMySql.ClientConnections.DataReaders.Ping;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlPingClientCommand:IClientCommand<MySqlPacketPayload>
{
    private readonly IConnectionSession _connectionSession;

    public MySqlPingClientCommand(IConnectionSession connectionSession)
    {
        _connectionSession = connectionSession;
    }
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        return new MySqlPingClientDataReader(_connectionSession);
    }
}