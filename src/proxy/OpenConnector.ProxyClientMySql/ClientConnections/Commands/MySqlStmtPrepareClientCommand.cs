using OpenConnector.Configuration.Session;
using OpenConnector.Protocol.MySql.Payload;
using OpenConnector.ProxyClient.Abstractions;
using OpenConnector.ProxyClientMySql.ClientConnections.DataReaders.StmtPrepare;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlStmtPrepareClientCommand:IClientCommand<MySqlPacketPayload>
{
    private readonly ConnectionSession _connectionSession;
    private readonly string _sql;
    public MySqlStmtPrepareClientCommand(MySqlPacketPayload payload,ConnectionSession connectionSession)
    {
        _connectionSession = connectionSession;
        _sql = payload.ReadStringEOF();
    }
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        return new MySqlStmtPrepareClientDataReader(_sql, _connectionSession);
    }
}