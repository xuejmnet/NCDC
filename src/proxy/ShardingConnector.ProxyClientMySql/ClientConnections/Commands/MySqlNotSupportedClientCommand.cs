using ShardingConnector.Protocol.MySql.Packet.Command;
using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClient.Exceptions;
using ShardingConnector.ProxyClientMySql.ClientConnections.DataReaders.NotSupport;

namespace ShardingConnector.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlNotSupportedClientCommand:IClientCommand<MySqlPacketPayload>
{
    private readonly MySqlCommandTypeEnum _commandType;

    public MySqlNotSupportedClientCommand(MySqlCommandTypeEnum commandType)
    {
        _commandType = commandType;
    }
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        return new MySqlNotSupportClientDataReader(_commandType);
    }
}