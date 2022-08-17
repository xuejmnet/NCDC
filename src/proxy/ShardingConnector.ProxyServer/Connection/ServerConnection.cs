using System.Data.Common;
using ShardingConnector.Base;
using ShardingConnector.ProxyServer.Session;
using ShardingConnector.Transaction;

namespace ShardingConnector.ProxyServer.Connection;

public class ServerConnection:IServerConnection,IDisposable
{
    private const int MAXIMUM_RETRY_COUNT = 5;


    private readonly ConnectionSession _connectionSession;
    private volatile string schemaName;


    private  TransactionTypeEnum _transactionType;
    private  bool _supportHint;
    public int ConnectionId { get; set; }
    public string UserName{ get; set; }

    private readonly MultiValueDictionary<string, DbConnection> _cachedConnections =
        new MultiValueDictionary<string, DbConnection>();
    private readonly ICollection<DbCommand> _cacheCommands 
        = new SynchronizedCollection<DbCommand>();
    private readonly ICollection<DbDataReader> _cacheDataReaders 
        = new SynchronizedCollection<DbDataReader>();

    public ServerConnection(ConnectionSession connectionSession)
    {
        _connectionSession = connectionSession;
    }

    public string SchemaName { get;private set; }
    public string LogicSchema { get; private set; }

    public void SetCurrentSchema(string schemaName)
    {
        //todo 判断
        SchemaName = schemaName;
        LogicSchema = schemaName;

    }
    public void Dispose()
    {
    }
}