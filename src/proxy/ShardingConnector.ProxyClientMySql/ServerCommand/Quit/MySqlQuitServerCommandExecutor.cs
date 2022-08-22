

using ShardingConnector.Protocol.MySql.Constant;
using ShardingConnector.Protocol.MySql.Packet.Generic;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient;

namespace ShardingConnector.ProxyClientMySql.ServerCommand.Quit;

public sealed class MySqlQuitServerCommandExecutor:ICommandExecutor
{
    public List<IPacket> Execute()
    {
        return new List<IPacket>(1)
        {
            new MySqlOkPacket(1, MySqlStatusFlagEnum.SERVER_STATUS_DEFAULT)
        };
    }
}