using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClientMySql.ClientConnections.DataReaders.StmtPrepare;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.ClientConnections.Commands;

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