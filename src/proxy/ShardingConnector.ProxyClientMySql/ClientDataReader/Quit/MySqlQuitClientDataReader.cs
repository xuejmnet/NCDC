using ShardingConnector.Protocol.MySql.Constant;
using ShardingConnector.Protocol.MySql.Packet.Generic;
using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient.Abstractions;

namespace ShardingConnector.ProxyClientMySql.ClientDataReader.Quit;

public class MySqlQuitClientDataReader:IClientDataReader<MySqlPacketPayload>
{
    public IEnumerable<IPacket<MySqlPacketPayload>> SendCommand()
    {
        return new List<IPacket<MySqlPacketPayload>>(1)
        {
            new MySqlOkPacket(1, MySqlStatusFlagEnum.SERVER_STATUS_DEFAULT)
        };
    }
}