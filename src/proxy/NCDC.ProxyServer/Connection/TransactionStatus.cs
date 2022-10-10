using NCDC.Enums;
using NCDC.Exceptions;

namespace NCDC.ProxyServer.Connection;

/// <summary>
/// 事务状态
/// </summary>
public sealed class TransactionStatus
{
    public  TransactionTypeEnum TransactionType { get; private set; }
    private  bool _inTransaction;

    public TransactionStatus(TransactionTypeEnum transactionType)
    {
        TransactionType = transactionType;
    }

    public void SetTransactionType(TransactionTypeEnum transactionType)
    {
        if (_inTransaction)
        {
            throw new ShardingException("failed to switch transaction type, please terminate current transaction.");
        }

        this.TransactionType = transactionType;
    }

    public bool IsInTransaction()
    {
        return _inTransaction;
    }
    public void SetInTransaction(bool inTransaction)
    {
         _inTransaction=inTransaction;
    }
    public bool IsInConnectionHeldTransaction()
    {
        return this._inTransaction && TransactionTypeEnum.BASE != this.TransactionType;
    }
    
}