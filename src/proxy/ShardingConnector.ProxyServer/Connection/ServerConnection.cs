using System.Data.Common;
using ShardingConnector.Base;
using ShardingConnector.Transaction;

namespace ShardingConnector.ProxyServer.Connection;

public class ServerConnection:IServerConnection,IDisposable
{
    private const int MAXIMUM_RETRY_COUNT = 5;

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

    public ServerConnection(TransactionTypeEnum transactionType):this(transactionType,false)
    {
        
    }
    public ServerConnection(TransactionTypeEnum transactionType,bool supportHint)
    {
        _transactionType = transactionType;
        _supportHint = supportHint;
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