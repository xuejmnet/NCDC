using ShardingConnector.Protocol.MySql.Constant;
using ShardingConnector.Protocol.MySql.Packet.Generic;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClientMySql.Common;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.ClientDataReader.SetOption;

public sealed class MySqlSetOptionClientDataReader:IClientDataReader
{
    private readonly MySqlSetOptionClientCommand _command;
    private readonly ConnectionSession _connectionSession;

    public MySqlSetOptionClientDataReader(MySqlSetOptionClientCommand command,ConnectionSession connectionSession)
    {
        _command = command;
        _connectionSession = connectionSession;
    }
    public List<IPacket>  SendCommand()
    {
        _connectionSession.AttributeMap.GetAttribute(MySqlConstants.MYSQL_OPTION_MULTI_STATEMENTS).Set(_command.Value);
        return new List<IPacket>()
        {
            new MySqlOkPacket(1, (MySqlStatusFlagEnum)ServerStatusFlagCalculator.CalculateFor(_connectionSession))
        };
    }
}