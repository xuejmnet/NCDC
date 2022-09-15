using NCDC.Protocol.MySql.Packet.Generic;
using NCDC.Protocol.MySql.Payload;
using NCDC.Protocol.Packets;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClientMySql.Common;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.DataReaders.Ping;

public sealed class MySqlPingClientDataReader : IClientDataReader<MySqlPacketPayload>
{
    private readonly IConnectionSession _connectionSession;

    public MySqlPingClientDataReader(IConnectionSession connectionSession)
    {
        _connectionSession = connectionSession;
    }

    public IEnumerable<IPacket<MySqlPacketPayload>> SendCommand()
    {
        return new List<IPacket<MySqlPacketPayload>>()
        {
            new MySqlOkPacket(1, ServerStatusFlagCalculator.CalculateFor(_connectionSession))
        };
    }
}