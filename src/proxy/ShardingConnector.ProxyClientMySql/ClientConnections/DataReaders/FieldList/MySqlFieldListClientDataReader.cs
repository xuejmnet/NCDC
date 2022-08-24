using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.ClientConnections.DataReaders.FieldList;

public sealed class MySqlFieldListClientDataReader:IClientDataReader<MySqlPacketPayload>
{
    private readonly string _table;
    private readonly string _filedWildcard;
    private readonly ConnectionSession _connectionSession;
    private readonly string _database;

    public MySqlFieldListClientDataReader(string table,string filedWildcard,ConnectionSession connectionSession)
    {
        _table = table;
        _filedWildcard = filedWildcard;
        _connectionSession = connectionSession;
        _database = connectionSession.GetDatabaseName()!;
    }
    public IEnumerable<IPacket<MySqlPacketPayload>> SendCommand()
    {
        throw new NotImplementedException();
    }
}