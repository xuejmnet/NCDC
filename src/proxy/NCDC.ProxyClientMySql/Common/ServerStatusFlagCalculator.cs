using NCDC.Protocol.MySql.Constant;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyClientMySql.Common;

public sealed class ServerStatusFlagCalculator
{
    public static MySqlStatusFlagEnum CalculateFor(IConnectionSession connectionSession)
    {
        int result = 0;
        result |= connectionSession.GetIsAutoCommit() ? (int)MySqlStatusFlagEnum.SERVER_STATUS_AUTOCOMMIT : 0;
        result |= connectionSession.GetTransactionStatus().IsInTransaction()
            ? (int)MySqlStatusFlagEnum.SERVER_STATUS_IN_TRANS
            : 0;
        return (MySqlStatusFlagEnum)result;
    }
}