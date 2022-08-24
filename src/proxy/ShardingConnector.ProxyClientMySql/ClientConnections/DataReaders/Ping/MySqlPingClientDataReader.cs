using ShardingConnector.Protocol.MySql.Packet.Generic;
using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClientMySql.Common;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.ClientConnections.DataReaders.Ping;

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