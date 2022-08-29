using OpenConnector.Protocol.MySql.Payload;
using OpenConnector.ProxyClient.Abstractions;
using OpenConnector.ProxyClientMySql.ClientConnections.DataReaders.Query;
using OpenConnector.ProxyServer.Abstractions;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlQueryClientCommand : IClientCommand<MySqlPacketPayload>
{
    private readonly ConnectionSession _connectionSession;
    private readonly IServerHandlerFactory _serverHandlerFactory;
    private readonly string _sql;

    public MySqlQueryClientCommand(MySqlPacketPayload payload,ConnectionSession connectionSession,IServerHandlerFactory serverHandlerFactory)
    {
        _connectionSession = connectionSession;
        _serverHandlerFactory = serverHandlerFactory;
        _sql = payload.ReadStringEOF();
    }

    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        return new MySqlQueryClientDataReader(_sql,_connectionSession,_serverHandlerFactory);
    }
}