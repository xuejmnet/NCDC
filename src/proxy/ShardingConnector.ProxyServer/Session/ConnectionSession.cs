using DotNetty.Common.Utilities;
using ShardingConnector.ProxyServer.Connection;
using ShardingConnector.ProxyServer.Session.Transaction;
using ShardingConnector.ShardingCommon.User;
using ShardingConnector.Transaction;

namespace ShardingConnector.ProxyServer.Session;

public class ConnectionSession
{
    private readonly TransactionStatus _transactionStatus;
    private volatile int _connectionId;
    private volatile Grantee _grantee;
    public ServerConnection ServerConnection { get; }
    public  IAttributeMap AttributeMap{ get; }
    private volatile bool autoCommit = true;
    private volatile string _databseName;
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

    public string GetDatabaseName()
    {
        return _databseName;
    }

    public void SetDatabaseName(string databaseName)
    {
        _databseName = databaseName;
    }
}