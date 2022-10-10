using System.Data;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.ServerHandlers.ServerTransactions;

public class LocalTransactionManager:ITransactionManager
{
    private readonly IConnectionSession _connectionSession;

    public LocalTransactionManager(IConnectionSession connectionSession)
    {
        _connectionSession = connectionSession;
    }
    public Task BeginAsync(IsolationLevel isolationLevel)
    {
        _connectionSession.GetConnectionInvokeReplays().Add(con=>con.BeginAsync(isolationLevel));
        return Task.CompletedTask;
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