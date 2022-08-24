using ShardingConnector.Protocol.MySql.Constant;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.Common;

public sealed class ServerStatusFlagCalculator
{
    public static MySqlStatusFlagEnum CalculateFor(ConnectionSession connectionSession)
    {
        int result = 0;
        result |= connectionSession.GetIsAutoCommit() ? (int)MySqlStatusFlagEnum.SERVER_STATUS_AUTOCOMMIT : 0;
        result |= connectionSession.GetTransactionStatus().InTransaction()
            ? (int)MySqlStatusFlagEnum.SERVER_STATUS_IN_TRANS
            : 0;
        return (MySqlStatusFlagEnum)result;
    }
}