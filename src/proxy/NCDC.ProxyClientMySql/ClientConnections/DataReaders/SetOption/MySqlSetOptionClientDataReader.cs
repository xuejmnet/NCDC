using NCDC.Protocol.MySql.Constant;
using NCDC.Protocol.MySql.Packet.Generic;
using NCDC.Protocol.MySql.Payload;
using NCDC.Protocol.Packets;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClientMySql.Common;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.DataReaders.SetOption;

public sealed class MySqlSetOptionClientDataReader:IClientDataReader<MySqlPacketPayload>
{
    private readonly int  _value;
    private readonly IConnectionSession _connectionSession;

    public MySqlSetOptionClientDataReader(int value,IConnectionSession connectionSession)
    {
        _value = value;
        _connectionSession = connectionSession;
    }
    public async IAsyncEnumerable<IPacket<MySqlPacketPayload>>  SendCommand()
    {
        _connectionSession.Channel.GetAttribute(MySqlConstants.MYSQL_OPTION_MULTI_STATEMENTS).Set(_value);
        
        var mySqlOkPacket = new MySqlOkPacket(1, ServerStatusFlagCalculator.CalculateFor(_connectionSession));
        yield return await Task.FromResult(mySqlOkPacket);
    }
}