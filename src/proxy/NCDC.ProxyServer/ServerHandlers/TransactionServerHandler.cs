using System.Data;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.ServerHandlers.Results;
using NCDC.ProxyServer.ServerHandlers.ServerTransactions;

namespace NCDC.ProxyServer.ServerHandlers;

public sealed class TransactionServerHandler:IServerHandler
{
    private readonly TransactionOperationTypeEnum _txType;
    private readonly IsolationLevel _isolationLevel;
    private readonly ServerTransactionManager _serverTransactionManager;

    public TransactionServerHandler(TransactionOperationTypeEnum txType,IConnectionSession connectionSession)
    {
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
            case TransactionOperationTypeEnum.BEGIN: await _serverTransactionManager.BeginAsync(_isolationLevel);break;
            case TransactionOperationTypeEnum.COMMIT: await _serverTransactionManager.CommitAsync();break;
            case TransactionOperationTypeEnum.ROLLBACK:await _serverTransactionManager.RollbackAsync();break;
            default: throw new NotSupportedException($"{_txType}");
        }

        return new RecordsAffectedServerResult();
    }
}