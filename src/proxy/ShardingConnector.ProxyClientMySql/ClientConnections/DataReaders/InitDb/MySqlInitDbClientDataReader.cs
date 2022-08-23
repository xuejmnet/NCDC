using ShardingConnector.CommandParser.Util;
using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.ClientConnections.DataReaders.InitDb;

public sealed class MySqlInitDbClientDataReader:IClientDataReader<MySqlPacketPayload>
{
    private readonly string _schema;
    private readonly ConnectionSession _connectionSession;

    public MySqlInitDbClientDataReader(string schema,ConnectionSession connectionSession)
    {
        _schema = schema;
        _connectionSession = connectionSession;
    }
    public IEnumerable<IPacket<MySqlPacketPayload>> SendCommand()
    {
        var exactlySchema = SqlUtil.GetExactlyValue(_schema);
        throw new NotImplementedException();
    }
}