using NCDC.Protocol.MySql.Payload;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClientMySql.ClientConnections.DataReaders.FieldList;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlFieldListClientCommand:IClientCommand<MySqlPacketPayload>
{
    private readonly IConnectionSession _connectionSession;
    private readonly IServerDataReaderFactory _serverDataReaderFactory;
    private readonly string _table;
    private readonly string _filedWildcard;
    public MySqlFieldListClientCommand(MySqlPacketPayload payload,IConnectionSession connectionSession,IServerDataReaderFactory serverDataReaderFactory)
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