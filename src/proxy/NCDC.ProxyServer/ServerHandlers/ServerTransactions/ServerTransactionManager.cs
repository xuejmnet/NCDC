using System.Data;
using NCDC.Enums;
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
            _connectionSession.CloseServerConnection();
        }
    }

    public Task CommitAsync()
    {
        throw new NotImplementedException();
    }

    public Task RollbackAsync()
    {
        throw new NotImplementedException();
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

    public bool SupportSavepoint { get; }
}