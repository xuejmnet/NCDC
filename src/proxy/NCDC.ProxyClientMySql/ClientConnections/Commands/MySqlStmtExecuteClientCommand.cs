using NCDC.Protocol.MySql.Payload;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlStmtExecuteClientCommand:IClientCommand<MySqlPacketPayload>
{
    private readonly IConnectionSession _connectionSession;

    public MySqlStmtExecuteClientCommand(MySqlPacketPayload payload,IConnectionSession connectionSession)
    {
        _connectionSession = connectionSession;
    }
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        throw new NotImplementedException();
    }
}