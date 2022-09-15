using NCDC.Protocol.MySql.Constant;
using NCDC.Protocol.MySql.Packet.Generic;
using NCDC.Protocol.MySql.Payload;
using NCDC.Protocol.Packets;
using NCDC.ProxyClient.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.DataReaders.Quit;

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