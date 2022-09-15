using NCDC.CommandParser.Abstractions;
using NCDC.Exceptions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Binaries;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.ServerHandlers;

public sealed class QueryServerHandler:IServerHandler
{
    private readonly ISqlCommand _sqlCommand;
    private readonly IConnectionSession _connectionSession;
    private readonly IServerDataReaderFactory _serverDataReaderFactory;
    public string Sql { get; }
    public IServerDataReader ServerDataReader { get; private set; }

    public QueryServerHandler(string sql,ISqlCommand sqlCommand,IConnectionSession connectionSession,IServerDataReaderFactory serverDataReaderFactory)
    {
        _sqlCommand = sqlCommand;
        _connectionSession = connectionSession;
        _serverDataReaderFactory = serverDataReaderFactory;
        Sql = sql;
    }
    public IServerResult Execute()
    {
        if (_connectionSession.LogicDatabase == null)
        {
            throw new ShardingException("no database selected");
        }

        ServerDataReader = _serverDataReaderFactory.Create(Sql,_sqlCommand, _connectionSession);
        return ServerDataReader.ExecuteDbDataReader();
        
       
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