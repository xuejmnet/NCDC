using ShardingConnector.Protocol.MySql.Constant;
using ShardingConnector.Protocol.MySql.Packet.Generic;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient.Abstractions;

namespace ShardingConnector.ProxyClientMySql.ClientDataReader.Quit;

public class MySqlQuitClientDataReader:IClientDataReader
{
    public List<IPacket> SendCommand()
    {
        return new List<IPacket>(1)
        {
            new MySqlOkPacket(1, MySqlStatusFlagEnum.SERVER_STATUS_DEFAULT)
        };
    }
}