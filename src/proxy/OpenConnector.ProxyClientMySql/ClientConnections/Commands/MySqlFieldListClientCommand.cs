using OpenConnector.Protocol.MySql.Payload;
using OpenConnector.ProxyClient.Abstractions;
using OpenConnector.ProxyClientMySql.ClientConnections.DataReaders.FieldList;
using OpenConnector.ProxyServer.Abstractions;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlFieldListClientCommand:IClientCommand<MySqlPacketPayload>
{
    private readonly ConnectionSession _connectionSession;
    private readonly IServerDataReaderFactory _serverDataReaderFactory;
    private readonly string _table;
    private readonly string _filedWildcard;
    public MySqlFieldListClientCommand(MySqlPacketPayload payload,ConnectionSession connectionSession,IServerDataReaderFactory serverDataReaderFactory)
    {
        _connectionSession = connectionSession;
        _serverDataReaderFactory = serverDataReaderFactory;
        _table = payload.ReadStringNul();
        _filedWildcard = payload.ReadStringEOF();
    }
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        return new MySqlFieldListClientDataReader(_table, _filedWildcard, _connectionSession, _serverDataReaderFactory);
    }
}