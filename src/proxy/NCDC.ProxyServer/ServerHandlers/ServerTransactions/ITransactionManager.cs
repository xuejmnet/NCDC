using System.Data;

namespace NCDC.ProxyServer.ServerHandlers.ServerTransactions;

public interface ITransactionManager
{
    Task BeginAsync(IsolationLevel isolationLevel);
    Task CommitAsync();
    Task RollbackAsync();
    Task CreateSavepoint(string name);
    Task RollbackToSavepoint(string name);
    Task ReleaseSavepoint(string name);
    bool SupportSavepoint { get; }
}