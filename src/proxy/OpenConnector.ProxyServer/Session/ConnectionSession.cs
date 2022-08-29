using DotNetty.Common.Utilities;
using OpenConnector.Base;
using OpenConnector.Exceptions;
using OpenConnector.ProxyServer.DatabaseInfo;
using OpenConnector.ProxyServer.Session.Connection;
using OpenConnector.ProxyServer.Session.Transaction;
using OpenConnector.ShardingCommon.User;
using OpenConnector.Transaction;

namespace OpenConnector.ProxyServer.Session;

public class ConnectionSession:IDisposable
{
    private readonly TransactionStatus _transactionStatus;
    private volatile int _connectionId;
    private volatile Grantee _grantee;
    public ServerConnection ServerConnection { get; }
    public  IAttributeMap AttributeMap{ get; }
    private volatile bool autoCommit = true;
    private volatile string? _databaseName;
    public string? DatabaseName => _databaseName;
    public LogicDatabase? LogicDatabase { get; private set; }
    private readonly TimeSpan _channelWaitMillis = TimeSpan.FromMilliseconds(200);

    private readonly ManualResetEventSlim _channelWaitWriteable = new ManualResetEventSlim(true);
 
    public ConnectionSession(TransactionTypeEnum transactionType,IAttributeMap attributeMap)
    {
        AttributeMap = attributeMap;
        _transactionStatus=new TransactionStatus(transactionType);
        ServerConnection = new ServerConnection(this);
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
        LogicDatabase = ProxyRuntimeContext.Instance.GetDatabase(databaseName!);
    }

    private void SubscribeChannelIsWritable()
    {
        _channelWaitWriteable.Reset();
     
    }
    public void WaitChannelIsWritable()
    {
        SubscribeChannelIsWritable();
        try
        {
            _channelWaitWriteable.Wait(_channelWaitMillis);
        }
        finally
        {
            NotifyChannelIsWritable();
        }
    }
    public void NotifyChannelIsWritable()
    {
        _channelWaitWriteable.Set();
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