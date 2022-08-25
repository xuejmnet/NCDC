using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using MySqlConnector;
using ShardingConnector.Base;
using ShardingConnector.ProxyServer.Session;
using ShardingConnector.Transaction;

namespace ShardingConnector.ProxyServer.Connection;

public class ServerConnection:IServerConnection,IDisposable
{
    private const int MAXIMUM_RETRY_COUNT = 5;


    private readonly ConnectionSession _connectionSession;


    private  TransactionTypeEnum _transactionType;
    private  bool _supportHint;
    public int ConnectionId { get; set; }
    public string UserName{ get; set; }

    private readonly Dictionary<string,List<DbConnection>>  _cachedConnections =
        new ();
    private readonly ConcurrentBag<DbCommand> _cacheCommands 
        = new();
    private readonly ConcurrentBag<DbDataReader> _cacheDataReaders 
        = new ();

    public ServerConnection(ConnectionSession connectionSession)
    {
        _connectionSession = connectionSession;
    }

    public void Dispose()
    {
    }
}