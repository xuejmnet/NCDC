using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClientMySql.ClientDataReader.Query;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.ClientConnections.Commands;

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