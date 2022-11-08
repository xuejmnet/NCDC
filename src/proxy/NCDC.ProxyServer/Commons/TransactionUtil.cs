using System.Data;
using NCDC.CommandParser.Common.Constant;

namespace NCDC.ProxyServer.Commons;

public class TransactionUtil
{
    public static IsolationLevel GetTransactionIsolationLevel(TransactionIsolationLevelEnum isolationLevel)
    {
        switch (isolationLevel)
        {
            case TransactionIsolationLevelEnum.READ_UNCOMMITTED: return IsolationLevel.ReadUncommitted;
            case TransactionIsolationLevelEnum.READ_COMMITTED: return IsolationLevel.ReadCommitted;
            case TransactionIsolationLevelEnum.REPEATABLE_READ: return IsolationLevel.RepeatableRead;
            case TransactionIsolationLevelEnum.SERIALIZABLE: return IsolationLevel.Serializable;
            case TransactionIsolationLevelEnum.SNAPSHOT: return IsolationLevel.Snapshot;
           default: return IsolationLevel.Unspecified;
        }
    }
}