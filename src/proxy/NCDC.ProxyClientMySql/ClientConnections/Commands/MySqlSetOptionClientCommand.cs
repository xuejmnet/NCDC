using NCDC.Protocol.MySql.Payload;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClientMySql.ClientConnections.DataReaders.SetOption;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlSetOptionClientCommand:IClientCommand<MySqlPacketPayload>
{
    public const int MYSQL_OPTION_MULTI_STATEMENTS_ON = 0;
    public const int MYSQL_OPTION_MULTI_STATEMENTS_OFF = 1;
    private readonly int _value;
    private readonly IConnectionSession _connectionSession;

    public MySqlSetOptionClientCommand(MySqlPacketPayload payload,IConnectionSession connectionSession)
    {
        _value = payload.ReadInt2();
        _connectionSession = connectionSession;
    }
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        return new MySqlSetOptionClientDataReader(_value, _connectionSession);
    }
}