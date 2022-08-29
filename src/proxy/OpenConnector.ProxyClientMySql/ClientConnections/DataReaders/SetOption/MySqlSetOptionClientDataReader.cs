using OpenConnector.Protocol.MySql.Constant;
using OpenConnector.Protocol.MySql.Packet.Generic;
using OpenConnector.Protocol.MySql.Payload;
using OpenConnector.Protocol.Packets;
using OpenConnector.ProxyClient.Abstractions;
using OpenConnector.ProxyClientMySql.Common;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyClientMySql.ClientConnections.DataReaders.SetOption;

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
            new MySqlOkPacket(1,ServerStatusFlagCalculator.CalculateFor(_connectionSession))
        };
    }
}