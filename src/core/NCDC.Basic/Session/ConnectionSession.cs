using DotNetty.Common.Utilities;
using NCDC.Basic.Connection;
using NCDC.Basic.Connection.Abstractions;
using NCDC.Basic.Connection.User;
using NCDC.Basic.Metadatas;
using OpenConnector.Transaction;

namespace NCDC.Basic.Session;

public class ConnectionSession:IConnectionSession,IDisposable
{
    private readonly TransactionStatus _transactionStatus;
    private volatile int _connectionId;
    private volatile Grantee _grantee;
    public IServerConnection ServerConnection { get; }
    public  IAttributeMap AttributeMap{ get; }
    private volatile bool autoCommit = true;
    private volatile string? _databaseName;
    public string? DatabaseName => _databaseName;
    public ILogicDatabase? LogicDatabase { get; private set; }
    private readonly TimeSpan _channelWaitMillis = TimeSpan.FromMilliseconds(200);

    private readonly ChannelIsWritableListener _channelWaitWriteableListener;
 
    public ConnectionSession(TransactionTypeEnum transactionType,IAttributeMap attributeMap)
    {
        AttributeMap = attributeMap;
        _transactionStatus=new TransactionStatus(transactionType);
        ServerConnection = new ServerConnection(this);
        _channelWaitWriteableListener = new ChannelIsWritableListener();
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
        // LogicDatabase = AppRuntimeContext.Instance.GetDatabase(databaseName!);
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