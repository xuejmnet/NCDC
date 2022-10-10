using NCDC.Protocol.MySql.Payload;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClientMySql.ClientConnections.DataReaders.ResetConnection;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.Commands;

public class MySqlResetConnectionCommand:IClientCommand<MySqlPacketPayload>
{
    private readonly IConnectionSession _connectionSession;

    public MySqlResetConnectionCommand(IConnectionSession connectionSession)
    {
        _connectionSession = connectionSession;
    }
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        return new MySqlResetConnectionDataReader(_connectionSession);
    }
}