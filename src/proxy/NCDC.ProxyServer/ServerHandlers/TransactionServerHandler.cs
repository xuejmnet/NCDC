using System.Data;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.ServerHandlers.Results;
using NCDC.ProxyServer.ServerHandlers.ServerTransactions;

namespace NCDC.ProxyServer.ServerHandlers;

public sealed class TransactionServerHandler:IServerHandler
{
    public IConnectionSession ConnectionSession { get; }
    private readonly TransactionOperationTypeEnum _txType;
    private readonly IsolationLevel _isolationLevel;
    private readonly ServerTransactionManager _serverTransactionManager;

    public TransactionServerHandler(TransactionOperationTypeEnum txType,IConnectionSession connectionSession)
    {
        ConnectionSession = connectionSession;
        _txType = txType;
        _isolationLevel = connectionSession.IsolationLevel;
        _serverTransactionManager = new ServerTransactionManager(connectionSession);
    }
    public Task<IServerResult> ExecuteAsync()
    {
        return DoTransactionAsync();
    }

    private async Task<IServerResult> DoTransactionAsync()
    {
        switch (_txType)
        {
            case TransactionOperationTypeEnum.BEGIN:await HandleBeginAsync();break;
            case TransactionOperationTypeEnum.COMMIT: await HandleCommitAsync();break;
            case TransactionOperationTypeEnum.ROLLBACK:await HandleRollbackAsync();break;
            default: throw new NotSupportedException($"{_txType}");
        }

        return RecordsAffectedServerResult.Default;
    }

    private async ValueTask HandleBeginAsync()
    {
        //如果之前在事务里面的直接把之前的提交上去
        if (ConnectionSession.GetTransactionStatus().IsInTransaction())
        {
            await _serverTransactionManager.CommitAsync();
        }
        await _serverTransactionManager.BeginAsync(_isolationLevel);
    }
    private async ValueTask HandleCommitAsync()
    {
        await _serverTransactionManager.CommitAsync();
    }
    private async ValueTask HandleRollbackAsync()
    {
        await _serverTransactionManager.RollbackAsync();
    }
}