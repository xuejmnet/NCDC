using ShardingConnector.Protocol.MySql.Packet.Command;
using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClient.Exceptions;

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
        throw new NotSupportedCommandException($"{_commandType}");
    }
}