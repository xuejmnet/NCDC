using NCDC.Protocol.MySql.Constant;
using NCDC.Protocol.MySql.Packet.Generic;
using NCDC.Protocol.MySql.Payload;
using NCDC.Protocol.Packets;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClientMySql.Common;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.DataReaders.ResetConnection;

public class MySqlResetConnectionDataReader:IClientDataReader<MySqlPacketPayload>
{
    private readonly IConnectionSession _connectionSession;

    public MySqlResetConnectionDataReader(IConnectionSession connectionSession)
    {
        _connectionSession = connectionSession;
    }
    public async IAsyncEnumerable<IPacket<MySqlPacketPayload>> SendCommand()
    {
        _connectionSession.Reset();
        var mySqlOkPacket = new MySqlOkPacket(1, ServerStatusFlagCalculator.CalculateFor(_connectionSession));
        yield return await Task.FromResult(mySqlOkPacket);
    }
}