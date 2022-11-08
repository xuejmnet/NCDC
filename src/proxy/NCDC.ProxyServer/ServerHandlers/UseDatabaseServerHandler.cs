using NCDC.CommandParser.Common.Util;
using NCDC.CommandParser.Dialect.Command.MySql.DAL;
using NCDC.Exceptions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.ServerHandlers.Results;

namespace NCDC.ProxyServer.ServerHandlers;

public sealed class UseDatabaseServerHandler:IServerHandler
{
    private readonly MySqlUseCommand _mySqlUseCommand;
    private readonly IConnectionSession _connectionSession;

    public UseDatabaseServerHandler( MySqlUseCommand mySqlUseCommand,IConnectionSession connectionSession)
    {
        _mySqlUseCommand = mySqlUseCommand;
        _connectionSession = connectionSession;
    }
    public Task<IServerResult> ExecuteAsync()
    {
        var database = SqlUtil.GetExactlyValue(_mySqlUseCommand.Schema);
        if (IsAuthorized(database))
        {
            return Task.FromResult((IServerResult)RecordsAffectedServerResult.Default);
        }

        throw new ShardingException($"unknown database {database}");
    }

    private bool IsAuthorized(string? database)
    {
        if (database is null)
        {
            return false;
        }
        if (_connectionSession.DatabaseExists(database))
        {
            // var logicDatabase = ProxyRuntimeContext.Instance.GetDatabase(database);
            // return logicDatabase!.UserNameAuthorize(_connectionSession.GetGrantee().Username);
        }

        return false;
    }
}