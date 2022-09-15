using NCDC.Protocol.MySql.Payload;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClientMySql.ClientConnections.DataReaders.StmtPrepare;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlStmtPrepareClientCommand:IClientCommand<MySqlPacketPayload>
{
    private readonly IConnectionSession _connectionSession;
    private readonly string _sql;
    public MySqlStmtPrepareClientCommand(MySqlPacketPayload payload,IConnectionSession connectionSession)
    {
        _connectionSession = connectionSession;
        _sql = payload.ReadStringEOF();
    }
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        return new MySqlStmtPrepareClientDataReader(_sql, _connectionSession);
    }
}