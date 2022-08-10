using DotNetty.Common.Utilities;
using ShardingConnector.ProxyServer.Session.Transaction;
using ShardingConnector.ShardingCommon.User;
using ShardingConnector.Transaction;

namespace ShardingConnector.ProxyServer.Session;

public class ConnectionSession
{
    private readonly IAttributeMap _attributeMap;
    private readonly TransactionStatus _transactionStatus;
    private volatile int _connectionId;
    private volatile Grantee _grantee;
    public ConnectionSession(TransactionTypeEnum transactionType,IAttributeMap attributeMap)
    {
        _attributeMap = attributeMap;
        _transactionStatus=new TransactionStatus(transactionType);
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
}