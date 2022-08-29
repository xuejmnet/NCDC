using OpenConnector.Exceptions;
using OpenConnector.Transaction;

namespace OpenConnector.ProxyServer.Session.Transaction;

/// <summary>
/// 事务状态
/// </summary>
public sealed class TransactionStatus
{
    private volatile TransactionTypeEnum _transactionType;
    private volatile bool _inTransaction;

    public TransactionStatus(TransactionTypeEnum transactionType)
    {
        _transactionType = transactionType;
    }

    public void SetTransactionType(TransactionTypeEnum transactionType)
    {
        if (_inTransaction)
        {
            throw new ShardingException("failed to switch transaction type, please terminate current transaction.");
        }

        this._transactionType = transactionType;
    }

    public bool InTransaction()
    {
        return _inTransaction;
    }
    public bool IsInConnectionHeldTransaction()
    {
        return this._inTransaction && TransactionTypeEnum.BASE != this._transactionType;
    }
    
}