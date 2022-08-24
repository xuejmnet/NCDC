using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlStmtExecuteClientCommand:IClientCommand<MySqlPacketPayload>
{
    private readonly ConnectionSession _connectionSession;

    public MySqlStmtExecuteClientCommand(MySqlPacketPayload payload,ConnectionSession connectionSession)
    {
        _connectionSession = connectionSession;
    }
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        throw new NotImplementedException();
    }
}