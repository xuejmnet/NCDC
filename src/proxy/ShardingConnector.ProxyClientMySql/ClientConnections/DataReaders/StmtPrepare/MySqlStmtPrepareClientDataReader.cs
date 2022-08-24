using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.ClientConnections.DataReaders.StmtPrepare;

public sealed class MySqlStmtPrepareClientDataReader:IClientDataReader<MySqlPacketPayload>
{
    private readonly string _sql;
    private readonly ConnectionSession _connectionSession;

    public MySqlStmtPrepareClientDataReader(string sql,ConnectionSession connectionSession)
    {
        _sql = sql;
        _connectionSession = connectionSession;
    }
    public IEnumerable<IPacket<MySqlPacketPayload>> SendCommand()
    {
        throw new NotImplementedException();
    }
}