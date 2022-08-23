using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClientMySql.ClientConnections.DataReaders.SetOption;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlSetOptionClientCommand:IClientCommand<MySqlPacketPayload>
{
    public const int MYSQL_OPTION_MULTI_STATEMENTS_ON = 0;
    public const int MYSQL_OPTION_MULTI_STATEMENTS_OFF = 1;
    private readonly int _value;
    private readonly ConnectionSession _connectionSession;

    public MySqlSetOptionClientCommand(MySqlPacketPayload payload,ConnectionSession connectionSession)
    {
        _value = payload.ReadInt2();
        _connectionSession = connectionSession;
    }
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        return new MySqlSetOptionClientDataReader(_value, _connectionSession);
    }
}