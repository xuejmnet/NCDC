using OpenConnector.Protocol.MySql.Payload;
using OpenConnector.ProxyClient.Abstractions;
using OpenConnector.ProxyClientMySql.ClientConnections.DataReaders.Quit;

namespace OpenConnector.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlQuitClientCommand:IClientCommand<MySqlPacketPayload>
{
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        return new MySqlQuitClientDataReader();
    }
}