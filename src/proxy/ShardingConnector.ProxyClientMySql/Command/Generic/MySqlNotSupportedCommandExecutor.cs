using ShardingConnector.Protocol.MySql.Packet.Command;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient;
using ShardingConnector.ProxyClient.Exceptions;

namespace ShardingConnector.ProxyClientMySql.Command.Generic;

public class MySqlNotSupportedCommandExecutor:ICommandExecutor
{
    private readonly MySqlCommandTypeEnum _commandTypeEnum;

    public MySqlNotSupportedCommandExecutor(MySqlCommandTypeEnum commandTypeEnum)
    {
        _commandTypeEnum = commandTypeEnum;
    }
    public List<IPacket> Execute()
    {
        throw new NotSupportedCommandException($"{_commandTypeEnum}");
    }
    public void Dispose()
    {
        
    }

}