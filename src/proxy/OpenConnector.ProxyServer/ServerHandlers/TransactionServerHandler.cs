using OpenConnector.ProxyServer.Abstractions;
using OpenConnector.ProxyServer.ServerHandlers.Results;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyServer.ServerHandlers;

public sealed class TransactionServerHandler:IServerHandler
{
    private readonly TransactionOperationTypeEnum _txType;
    private readonly ConnectionSession _connectionSession;

    public TransactionServerHandler(TransactionOperationTypeEnum txType,ConnectionSession connectionSession)
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