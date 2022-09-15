using NCDC.Protocol.MySql.Payload;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClientMySql.ClientConnections.DataReaders.Quit;

namespace NCDC.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlQuitClientCommand:IClientCommand<MySqlPacketPayload>
{
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        return new MySqlQuitClientDataReader();
    }
}