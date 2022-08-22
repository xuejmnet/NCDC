using ShardingConnector.Protocol.MySql.Constant;
using ShardingConnector.Protocol.MySql.Packet.Generic;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient;
using ShardingConnector.ProxyServer.Commands;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.Command.Admin;

public class MySqlComSetOptionExecutor:ICommandExecutor
{
    private readonly MySqlServerComSetOptionPacket _packet;
    private readonly ConnectionSession _connectionSession;

    public MySqlComSetOptionExecutor(MySqlServerComSetOptionPacket packet,ConnectionSession connectionSession)
    {
        _packet = packet;
        _connectionSession = connectionSession;
    }
    public List<IPacket> Execute()
    {
        _connectionSession.AttributeMap.GetAttribute(MySqlConstants.MYSQL_OPTION_MULTI_STATEMENTS).Set(_packet.Value);
        return new List<IPacket>()
        {
            new MySqlOkPacket(1, (MySqlStatusFlagEnum)ServerStatusFlagCalculator.CalculateFor(_connectionSession))
        };
    }
    public void Dispose()
    {
        
    }
}