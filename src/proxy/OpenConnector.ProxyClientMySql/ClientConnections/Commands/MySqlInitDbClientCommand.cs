using OpenConnector.Configuration.Session;
using OpenConnector.Protocol.MySql.Payload;
using OpenConnector.ProxyClient.Abstractions;
using OpenConnector.ProxyClientMySql.ClientConnections.DataReaders.InitDb;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlInitDbClientCommand:IClientCommand<MySqlPacketPayload>
{
    private readonly ConnectionSession _connectionSession;
    private readonly string _schema;
    public MySqlInitDbClientCommand(MySqlPacketPayload payload,ConnectionSession connectionSession)
    {
        _connectionSession = connectionSession;
        _schema = payload.ReadStringEOF();
    }
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        return new MySqlInitDbClientDataReader(_schema, _connectionSession);
    }
}