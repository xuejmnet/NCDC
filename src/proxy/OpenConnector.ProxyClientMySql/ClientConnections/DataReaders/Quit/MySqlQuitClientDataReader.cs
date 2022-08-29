using OpenConnector.Protocol.MySql.Constant;
using OpenConnector.Protocol.MySql.Packet.Generic;
using OpenConnector.Protocol.MySql.Payload;
using OpenConnector.Protocol.Packets;
using OpenConnector.ProxyClient.Abstractions;

namespace OpenConnector.ProxyClientMySql.ClientConnections.DataReaders.Quit;

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