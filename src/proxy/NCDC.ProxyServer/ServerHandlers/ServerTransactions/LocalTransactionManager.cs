using System.Data;
using NCDC.Exceptions;
using NCDC.Extensions;
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
        _connectionSession.ServerConnection.RecordTargetMethodInvoke(con=>con.BeginAsync(isolationLevel));
        return Task.CompletedTask;
    }

    public async Task CommitAsync()
    {
        if (_connectionSession.GetTransactionStatus().IsInTransaction())
        {
            var rollbackConnections = await CommitConnectionsAsync();
            ThrowAggregateExceptions(rollbackConnections);
        }
    }

    public async Task RollbackAsync()
    {
        if (_connectionSession.GetTransactionStatus().IsInTransaction())
        {
            var rollbackConnections = await RollbackConnectionsAsync();
            ThrowAggregateExceptions(rollbackConnections);
        }
    }

    private void ThrowAggregateExceptions(ICollection<Exception> exceptions)
    {
        if (exceptions.IsEmpty())
            return;
        throw new AggregateException(exceptions);
    }

    private async Task<ICollection<Exception>> RollbackConnectionsAsync()
    {
        var result = new LinkedList<Exception>();
        foreach (var serverConnectionCachedConnection in _connectionSession.ServerConnection.CachedConnections)
        {
            foreach (var serverDbConnection in serverConnectionCachedConnection.Value)
            {
                try
                {
                    await serverDbConnection.RollbackAsync();
                }
                catch (Exception e)
                {
                    result.AddLast(e);
                }
            }
        }
        return result;
    }
    private async Task<ICollection<Exception>> CommitConnectionsAsync()
    {
        var result = new LinkedList<Exception>();
        foreach (var serverConnectionCachedConnection in _connectionSession.ServerConnection.CachedConnections)
        {
            foreach (var serverDbConnection in serverConnectionCachedConnection.Value)
            {
                try
                {
                    await serverDbConnection.CommitAsync();
                }
                catch (Exception e)
                {
                    result.AddLast(e);
                }
            }
        }
        return result;
    }

    public Task CreateSavepoint(string name)
    {
        throw new NotSupportedException(name);
    }

    public Task RollbackToSavepoint(string name)
    {
        throw new NotSupportedException(name);
    }

    public Task ReleaseSavepoint(string name)
    {
        throw new NotSupportedException(name);
    }

    public bool SupportSavepoint => false;
}