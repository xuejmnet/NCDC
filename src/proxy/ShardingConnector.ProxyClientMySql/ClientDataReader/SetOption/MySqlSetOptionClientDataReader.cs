using ShardingConnector.Protocol.MySql.Constant;
using ShardingConnector.Protocol.MySql.Packet.Generic;
using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClientMySql.Common;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.ClientDataReader.SetOption;

public sealed class MySqlSetOptionClientDataReader:IClientDataReader<MySqlPacketPayload>
{
    private readonly int  _value;
    private readonly ConnectionSession _connectionSession;

    public MySqlSetOptionClientDataReader(int value,ConnectionSession connectionSession)
    {
        _value = value;
        _connectionSession = connectionSession;
    }
    public IEnumerable<IPacket<MySqlPacketPayload>>  SendCommand()
    {
        _connectionSession.AttributeMap.GetAttribute(MySqlConstants.MYSQL_OPTION_MULTI_STATEMENTS).Set(_value);
        return new List<IPacket<MySqlPacketPayload>>()
        {
            new MySqlOkPacket(1, (MySqlStatusFlagEnum)ServerStatusFlagCalculator.CalculateFor(_connectionSession))
        };
    }
}