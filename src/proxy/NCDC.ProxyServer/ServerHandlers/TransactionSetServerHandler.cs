using System.Data;
using NCDC.CommandParser.Common.Command.TCL;
using NCDC.CommandParser.Common.Constant;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Commons;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.ServerHandlers.Results;

namespace NCDC.ProxyServer.ServerHandlers;

public class TransactionSetServerHandler:IServerHandler
{
    private readonly SetTransactionCommand _setTransactionCommand;
    private readonly IConnectionSession _connectionSession;

    public TransactionSetServerHandler(SetTransactionCommand setTransactionCommand,IQueryContext queryContext)
    {
        _setTransactionCommand = setTransactionCommand;
        _connectionSession = queryContext.ConnectionSession;
    }
    public Task<IServerResult> ExecuteAsync()
    {
        var isolationLevel = _setTransactionCommand.IsolationLevel;
        if ( isolationLevel.HasValue)
        {
            _connectionSession.IsolationLevel = TransactionUtil.GetTransactionIsolationLevel(isolationLevel.Value);
        }
        return Task.FromResult((IServerResult)RecordsAffectedServerResult.Default);
    }
}