using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.Exceptions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Binaries;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.ServerHandlers;

public sealed class QueryServerHandler:IServerHandler
{
    private readonly IQueryContext _queryContext;
    private readonly IServerDataReaderFactory _serverDataReaderFactory;
    public IServerDataReader ServerDataReader { get; private set; }

    public QueryServerHandler(IQueryContext queryContext,IServerDataReaderFactory serverDataReaderFactory)
    {
        _queryContext = queryContext;
        _serverDataReaderFactory = serverDataReaderFactory;
    }
    public Task<IServerResult> ExecuteAsync()
    {
        // if (_connectionSession.VirtualDataSource == null)
        // {
        //     throw new ShardingException("no database selected");
        // }

        ServerDataReader = _serverDataReaderFactory.Create(_queryContext);
        return ServerDataReader.ExecuteDbDataReaderAsync();
    }




    public bool Read()
    {
        return ServerDataReader!.Read();
    }

    public BinaryRow GetRowData()
    {
        return ServerDataReader!.GetRowData();
    }

    public void Dispose()
    {
        ServerDataReader.Dispose();
    }
}