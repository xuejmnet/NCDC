using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.ServerHandlers.Results;

namespace NCDC.ProxyServer.ServerHandlers;

public sealed class TransactionServerHandler:IServerHandler
{
    private readonly TransactionOperationTypeEnum _txType;
    private readonly IConnectionSession _connectionSession;

    public TransactionServerHandler(TransactionOperationTypeEnum txType,IConnectionSession connectionSession)
    {
        _txType = txType;
        _connectionSession = connectionSession;
    }
    public IServerResult Execute()
    {
        return DoTransaction();
    }

    private IServerResult DoTransaction()
    {
        switch (_txType)
        {
            case TransactionOperationTypeEnum.BEGIN:break;
            case TransactionOperationTypeEnum.COMMIT:break;
            case TransactionOperationTypeEnum.ROLLBACK:break;
            default: throw new NotSupportedException($"{_txType}");
        }

        return new RecordsAffectedServerResult();
    }
}