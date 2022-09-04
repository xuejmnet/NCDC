using OpenConnector.Configuration.Session;
using OpenConnector.Exceptions;
using OpenConnector.Extensions;
using OpenConnector.ProxyServer.Abstractions;
using OpenConnector.ProxyServer.Binaries;
using OpenConnector.ProxyServer.ServerHandlers.Results;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyServer.ServerHandlers;

/// <summary>
/// 单播到当前database的所有的数据源
/// </summary>
public sealed class UnicastServerHandler : IServerHandler
{
    private readonly string _sql;
    private readonly ConnectionSession _connectionSession;
    private readonly IServerDataReaderFactory _serverDataReaderFactory;
    private IServerDataReader? _serverDataReader;

    public UnicastServerHandler(string sql, ConnectionSession connectionSession,IServerDataReaderFactory serverDataReaderFactory)
    {
        _sql = sql;
        _connectionSession = connectionSession;
        _serverDataReaderFactory = serverDataReaderFactory;
    }

    public IServerResult Execute()
    {
        var originalDatabaseName = _connectionSession.DatabaseName;
        var currentDatabaseName = originalDatabaseName ?? GetFirstDatabaseName();
        try
        {
            _connectionSession.SetCurrentDatabaseName(currentDatabaseName);
            _serverDataReader = _serverDataReaderFactory.Create(_sql, _connectionSession);
                return _serverDataReader.ExecuteDbDataReader();
        }
        finally
        {
            _connectionSession.SetCurrentDatabaseName(originalDatabaseName);
        }
    }

    private string GetFirstDatabaseName()
    {
        var allDatabaseNames = ProxyRuntimeContext.Instance.GetAllDatabaseNames();

        return allDatabaseNames.FirstOrDefault() ?? throw new ShardingException("no database can select");
    }

    public bool Read()
    {
        return _serverDataReader!.Read();
    }

    public BinaryRow GetRowData()
    {
        return _serverDataReader!.GetRowData();
    }

    public void Dispose()
    {
        _serverDataReader?.Dispose();
    }
}