using System.Data.Common;
using MySqlConnector;
using ShardingConnector.Exceptions;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Binaries;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyServer.ServerHandlers;

public sealed class QueryServerHandler:IServerHandler
{
    private readonly IServerDataReaderFactory _serverDataReaderFactory;
    public string Sql { get; }
    public ConnectionSession ConnectionSession { get; }
    public IServerDataReader ServerDataReader { get; private set; }

    public QueryServerHandler(string sql,ConnectionSession connectionSession,IServerDataReaderFactory serverDataReaderFactory)
    {
        _serverDataReaderFactory = serverDataReaderFactory;
        Sql = sql;
        ConnectionSession = connectionSession;
    }
    public IServerResult Execute()
    {
        if (ConnectionSession.LogicDatabase == null)
        {
            throw new ShardingException("no database selected");
        }

        ServerDataReader = _serverDataReaderFactory.Create(Sql, ConnectionSession);
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