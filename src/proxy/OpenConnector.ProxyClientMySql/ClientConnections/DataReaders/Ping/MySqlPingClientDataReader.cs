using OpenConnector.Protocol.MySql.Packet.Generic;
using OpenConnector.Protocol.MySql.Payload;
using OpenConnector.Protocol.Packets;
using OpenConnector.ProxyClient.Abstractions;
using OpenConnector.ProxyClientMySql.Common;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyClientMySql.ClientConnections.DataReaders.Ping;

public sealed class MySqlPingClientDataReader : IClientDataReader<MySqlPacketPayload>
{
    private readonly ConnectionSession _connectionSession;

    public MySqlPingClientDataReader(ConnectionSession connectionSession)
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