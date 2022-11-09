using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.Exceptions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Binaries;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.ServerHandlers;

public sealed class QueryServerHandler:IServerHandler
{
    private readonly IConnectionSession _connectionSession;
    private readonly IServerDataReaderFactory _serverDataReaderFactory;
    public IServerDataReader ServerDataReader { get; private set; }

    public QueryServerHandler(IConnectionSession connectionSession,IServerDataReaderFactory serverDataReaderFactory)
    {
        _connectionSession = connectionSession;
        _serverDataReaderFactory = serverDataReaderFactory;
    }
    public Task<IServerResult> ExecuteAsync()
    {
        if (_connectionSession.VirtualDataSource == null)
        {
            throw new ShardingException("no database selected");
        }

        ServerDataReader = _serverDataReaderFactory.Create(_connectionSession);
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