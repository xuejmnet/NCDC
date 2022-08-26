using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using MySqlConnector;
using ShardingConnector.Base;
using ShardingConnector.Executor.Constant;
using ShardingConnector.Extensions;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Session;
using ShardingConnector.Transaction;

namespace ShardingConnector.ProxyServer.Connection;

public class ServerConnection:IServerConnection,IDisposable,IAdoMethodReplier<IServerDbConnection>
{
    private const int MAXIMUM_RETRY_COUNT = 5;


    private  TransactionTypeEnum _transactionType;
    private  bool _supportHint;
    public int ConnectionId { get; set; }
    public string UserName{ get; set; }

    private readonly Dictionary<string,List<IServerDbConnection>>  _cachedConnections =
        new ();

    public ServerConnection(ConnectionSession connectionSession)
    {
        _transactionType = TransactionTypeEnum.LOCAL;
        ConnectionSession = connectionSession;
        Replier = new LinkedList<Action<IServerDbConnection>>();
    }

    public void Dispose()
    {
    }

    public ConnectionSession ConnectionSession { get; }

    public List<IServerDbConnection> GetConnections(ConnectionModeEnum connectionMode,string dataSourceName,  int connectionSize)
    {
        var serverDbConnections = GetServerDbConnectionFromContext(connectionMode,dataSourceName,connectionSize);
        lock (_cachedConnections)
        {
            if (!_cachedConnections.TryGetValue(dataSourceName, out var cached))
            {
                cached = new List<IServerDbConnection>(Math.Max(serverDbConnections.Count * 2,Environment.ProcessorCount));
                _cachedConnections.TryAdd(dataSourceName, cached);
            }
            cached.AddAll(serverDbConnections);
        }

        return serverDbConnections;
    }

    private List<IServerDbConnection> GetServerDbConnectionFromContext(ConnectionModeEnum connectionMode,
        string dataSourceName, int connectionSize)
    {
        if (ConnectionSession.LogicDatabase == null)
        {
            throw new ArgumentException("current logic database is null");
        }

        return ConnectionSession.LogicDatabase.GetServerDbConnections(connectionMode, dataSourceName, connectionSize,
            _transactionType);
    }

    public LinkedList<Action<IServerDbConnection>> Replier { get; }
}