using NCDC.CommandParser.Command.DAL.Dialect.MySql;
using NCDC.CommandParser.Util;
using NCDC.Exceptions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.ServerHandlers.Results;

namespace NCDC.ProxyServer.ServerHandlers;

public sealed class UseDatabaseServerHandler:IServerHandler
{
    private readonly UseCommand _useCommand;
    private readonly IConnectionSession _connectionSession;

    public UseDatabaseServerHandler( UseCommand useCommand,IConnectionSession connectionSession)
    {
        _useCommand = useCommand;
        _connectionSession = connectionSession;
    }
    public Task<IServerResult> ExecuteAsync()
    {
        var database = SqlUtil.GetExactlyValue(_useCommand.GetSchema());
        if (IsAuthorized(database))
        {
            return Task.FromResult((IServerResult)RecordsAffectedServerResult.Empty);
        }

        throw new ShardingException($"unknown database {database}");
    }

    private bool IsAuthorized(string database)
    {
        if (_connectionSession.DatabaseExists(database))
        {
            // var logicDatabase = ProxyRuntimeContext.Instance.GetDatabase(database);
            // return logicDatabase!.UserNameAuthorize(_connectionSession.GetGrantee().Username);
        }

        return false;
    }
}