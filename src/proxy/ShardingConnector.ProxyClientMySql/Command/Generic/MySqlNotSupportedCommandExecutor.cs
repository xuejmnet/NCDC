using ShardingConnector.ProtocolCore.Packets;
using ShardingConnector.ProtocolCore.Packets.Executor;
using ShardingConnector.ProtocolMysql.Packet.Command;
using ShardingConnector.ProxyClient.Exceptions;

namespace ShardingConnector.ProxyClientMySql.Command.Generic;

public class MySqlNotSupportedCommandExecutor:ICommandExecutor
{
    private readonly MySqlCommandTypeEnum _commandTypeEnum;

    public MySqlNotSupportedCommandExecutor(MySqlCommandTypeEnum commandTypeEnum)
    {
        _commandTypeEnum = commandTypeEnum;
    }
    public List<IDatabasePacket> Execute()
    {
        throw new NotSupportedCommandException($"{_commandTypeEnum}");
    }
    public void Dispose()
    {
        
    }

}