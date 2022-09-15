using NCDC.Protocol.MySql.Payload;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClientMySql.ClientConnections.DataReaders.Query;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlQueryClientCommand : IClientCommand<MySqlPacketPayload>
{
    private readonly IConnectionSession _connectionSession;
    private readonly IServerHandlerFactory _serverHandlerFactory;
    private readonly string _sql;

    public MySqlQueryClientCommand(MySqlPacketPayload payload,IConnectionSession connectionSession,IServerHandlerFactory serverHandlerFactory)
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