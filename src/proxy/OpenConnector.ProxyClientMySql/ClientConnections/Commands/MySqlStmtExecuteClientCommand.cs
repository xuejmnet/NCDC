using OpenConnector.Configuration.Session;
using OpenConnector.Protocol.MySql.Payload;
using OpenConnector.ProxyClient.Abstractions;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyClientMySql.ClientConnections.Commands;

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