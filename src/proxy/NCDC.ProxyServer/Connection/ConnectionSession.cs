using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using NCDC.Basic.Metadatas;
using NCDC.Enums;
using NCDC.ProxyServer.AppServices;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.Connection.Metadatas;
using NCDC.ProxyServer.Connection.User;
using NCDC.ProxyServer.Contexts;
using NCDC.ProxyServer.Runtimes;

namespace NCDC.ProxyServer.Connection;

public class ConnectionSession:IConnectionSession
{
    private readonly TransactionStatus _transactionStatus;
    private volatile int _connectionId;
    private volatile Grantee _grantee;
    public IServerConnection ServerConnection { get; }


    public  IChannel Channel{ get; }
    public ILogicDatabase? LogicDatabase => RuntimeContext?.GetDatabase();
    private volatile bool autoCommit = true;
    private volatile string? _databaseName;
    public string? DatabaseName => _databaseName;
    public IRuntimeContext? RuntimeContext { get;private set; }
    public IAppRuntimeManager AppRuntimeManager { get; }
    private readonly TimeSpan _channelWaitMillis = TimeSpan.FromMilliseconds(200);

    private readonly ChannelIsWritableListener _channelWaitWriteableListener;
 
    public ConnectionSession(TransactionTypeEnum transactionType,IChannel channel,IAppRuntimeManager appRuntimeManager)
    {
        AppRuntimeManager = appRuntimeManager;
        Channel = channel;
        _transactionStatus=new TransactionStatus(transactionType);
        ServerConnection = new ServerConnection(this);
        _channelWaitWriteableListener = new ChannelIsWritableListener();
    }

    public ICollection<string> GetAllDatabaseNames()
    {
        return AppRuntimeManager.GetAllDatabaseNames();
    }

    public ICollection<string> GetAuthorizeDatabases()
    {
        return AppRuntimeManager.GetAuthorizedDatabases(_grantee.Username);
    }

    public bool DatabaseExists(string database)
    {
        return AppRuntimeManager.ContainsRuntimeContext(database);
    }


    public bool GetIsAutoCommit()
    {
        return autoCommit;
    }

    public TransactionStatus GetTransactionStatus()
    {
        return _transactionStatus;
    }

    public int GetConnectionId()
    {
        return _connectionId;
    }

    public void SetConnectionId(int connectionId)
    {
        this._connectionId = connectionId;
    }

    public Grantee GetGrantee()
    {
        return _grantee;
    }

    public void SetGrantee(Grantee grantee)
    {
        _grantee = grantee;
    }

    public void SetCurrentDatabaseName(string? databaseName)
    {
        if (databaseName != null && databaseName == _databaseName)
        {
            return;
        }
        //todo 判断是否在事务中
        _databaseName = databaseName;
        RuntimeContext =AppRuntimeManager.GetRuntimeContext(databaseName!);
    }
    public  Task WaitChannelIsWritableAsync(CancellationToken cancellationToken=default)
    {
        return  _channelWaitWriteableListener.WaitAsync(_channelWaitMillis,cancellationToken);
    }
    public void NotifyChannelIsWritable()
    {
        _channelWaitWriteableListener.Wakeup();
    }

    public void CloseServerConnection()
    {
        ServerConnection.CloseCurrentCommandReader();
    }

    public void Dispose()
    {
        ServerConnection.Dispose();
    }
}