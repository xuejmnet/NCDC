using OpenConnector.Protocol.MySql.Payload;
using OpenConnector.Protocol.Packets;
using OpenConnector.ProxyClient.Abstractions;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyClientMySql.ClientConnections.DataReaders.StmtPrepare;

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