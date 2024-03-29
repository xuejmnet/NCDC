using NCDC.Exceptions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Binaries;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.ServerHandlers;

/// <summary>
/// 单播到当前database的所有的数据源
/// </summary>
public sealed class UnicastServerHandler : IServerHandler
{
    private readonly IQueryContext _queryContext;
    private readonly IServerDataReaderFactory _serverDataReaderFactory;
    private IServerDataReader? _serverDataReader;

    public UnicastServerHandler(IQueryContext queryContext,
        IServerDataReaderFactory serverDataReaderFactory)
    {
        _queryContext = queryContext;
        _serverDataReaderFactory = serverDataReaderFactory;
    }

    public async Task<IServerResult> ExecuteAsync()
    {
        _serverDataReader = _serverDataReaderFactory.Create(_queryContext);
        return await _serverDataReader.ExecuteDbDataReaderAsync();
    }

    // private string GetFirstDatabaseName()
    // {
    //     var allDatabaseNames = _connectionSession.GetAllDatabaseNames();
    //
    //     return allDatabaseNames.FirstOrDefault() ?? throw new ShardingException("no database can select");
    // }

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