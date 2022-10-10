using System.Data;
using NCDC.Enums;
using NCDC.Exceptions;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.ServerHandlers.ServerTransactions;

public sealed class ServerTransactionManager:ITransactionManager
{
    private readonly IConnectionSession _connectionSession;
    private readonly TransactionTypeEnum _transactionType;
    private readonly LocalTransactionManager _localTransactionManager;

    public ServerTransactionManager(IConnectionSession connectionSession)
    {
        _connectionSession = connectionSession;
        _transactionType = connectionSession.GetTransactionStatus().TransactionType;
        _localTransactionManager=new LocalTransactionManager(connectionSession);
    }
    public async Task BeginAsync(IsolationLevel isolationLevel)
    {
        //如果当前没有在事务里面那么就设置当前在事务里面
        if (!_connectionSession.GetTransactionStatus().IsInTransaction())
        {
            _connectionSession.GetTransactionStatus().SetInTransaction(true);
            TransactionHolder.InTransaction = true;
            await _connectionSession.ServerConnection.ReleaseConnectionsAsync(false);
        }

        if (TransactionTypeEnum.LOCAL == _transactionType)
        {
            await _localTransactionManager.BeginAsync(isolationLevel);
            return;
        }

        throw new ShardingNotSupportedException($"transaction type:{_transactionType}");
    }

    public async Task CommitAsync()
    {
        if (_connectionSession.GetTransactionStatus().IsInTransaction())
        {
            try
            {
                if (TransactionTypeEnum.LOCAL == _transactionType)
                {
                    await _localTransactionManager.CommitAsync();
                    return;
                }
                throw new ShardingNotSupportedException($"transaction type:{_transactionType}");
            }
            finally
            {
                _connectionSession.GetTransactionStatus().SetInTransaction(false);
                TransactionHolder.InTransaction = null;
            }
        }
    }

    public async Task RollbackAsync()
    {
        if (_connectionSession.GetTransactionStatus().IsInTransaction())
        {
            try
            {
                if (TransactionTypeEnum.LOCAL == _transactionType)
                {
                    await _localTransactionManager.RollbackAsync();
                    return;
                }
                throw new ShardingNotSupportedException($"transaction type:{_transactionType}");
            }
            finally
            {
                _connectionSession.GetTransactionStatus().SetInTransaction(false);
                TransactionHolder.InTransaction = null;
            }
        }
    }

    public Task CreateSavepoint(string name)
    {
        throw new NotImplementedException();
    }

    public Task RollbackToSavepoint(string name)
    {
        throw new NotImplementedException();
    }

    public Task ReleaseSavepoint(string name)
    {
        throw new NotImplementedException();
    }

    public bool SupportSavepoint => false;
}