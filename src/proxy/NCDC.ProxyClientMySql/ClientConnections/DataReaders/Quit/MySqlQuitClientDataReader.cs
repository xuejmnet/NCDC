using NCDC.Protocol.MySql.Constant;
using NCDC.Protocol.MySql.Packet.Generic;
using NCDC.Protocol.MySql.Payload;
using NCDC.Protocol.Packets;
using NCDC.ProxyClient.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.DataReaders.Quit;

public class MySqlQuitClientDataReader:IClientDataReader<MySqlPacketPayload>
{
    public async IAsyncEnumerable<IPacket<MySqlPacketPayload>> SendCommand()
    {
        var mySqlOkPacket = new MySqlOkPacket(1, MySqlStatusFlagEnum.SERVER_STATUS_DEFAULT);
        yield return await Task.FromResult(mySqlOkPacket);
    }
}