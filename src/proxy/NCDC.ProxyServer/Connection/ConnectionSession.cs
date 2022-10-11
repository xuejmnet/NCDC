using System.Data;
using DotNetty.Transport.Channels;
using NCDC.Basic.User;
using NCDC.CommandParser.Abstractions;
using NCDC.Enums;
using NCDC.Exceptions;
using NCDC.Extensions;
using NCDC.ProxyServer.AppServices.Abstractions;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.Databases;
using NCDC.ProxyServer.Runtimes;

namespace NCDC.ProxyServer.Connection;

public class ConnectionSession : IConnectionSession
{
    private readonly ISqlCommandParser _sqlCommandParser;
    private readonly TransactionStatus _transactionStatus;
    private volatile int _connectionId;
    private volatile Grantee _grantee;

    public IServerConnection ServerConnection { get; }


    public IChannel Channel { get; }

    // public ILogicDatabase? LogicDatabase => RuntimeContext?.GetDatabase();
    public IVirtualDataSource? VirtualDataSource => RuntimeContext?.GetVirtualDataSource();
    private volatile bool autoCommit = true;
    private volatile string? _databaseName;
    public IsolationLevel IsolationLevel { get; private set; }
    public string? DatabaseName => _databaseName;
    public IRuntimeContext? RuntimeContext { get; private set; }
    public IAppRuntimeManager AppRuntimeManager { get; }
    private readonly TimeSpan _channelWaitMillis = TimeSpan.FromMilliseconds(200);

    private readonly ChannelIsWritableListener _channelWaitWriteableListener;

    public ConnectionSession(TransactionTypeEnum transactionType, IsolationLevel isolationLevel, IChannel channel,
        IAppRuntimeManager appRuntimeManager,ISqlCommandParser sqlCommandParser)
    {
        _sqlCommandParser = sqlCommandParser;
        IsolationLevel = isolationLevel;
        AppRuntimeManager = appRuntimeManager;
        Channel = channel;
        _transactionStatus = new TransactionStatus(transactionType);
        ServerConnection = new ServerConnection(this);
        _channelWaitWriteableListener = new ChannelIsWritableListener();
    }
    public ISqlCommandParser GetSqlCommandParser()
    {
        return _sqlCommandParser;
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

        if (_transactionStatus.IsInTransaction())
        {
            throw new ShardingException("Failed to switch database, please terminate current transaction.");
        }

        _databaseName = databaseName;
        RuntimeContext = databaseName.IsNullOrWhiteSpace() ? null : AppRuntimeManager.GetRuntimeContext(databaseName!);
    }

    public Task WaitChannelIsWritableAsync(CancellationToken cancellationToken = default)
    {
        return _channelWaitWriteableListener.WaitAsync(_channelWaitMillis, cancellationToken);
    }

    public void NotifyChannelIsWritable()
    {
        _channelWaitWriteableListener.Wakeup();
    }


    public void Reset()
    {
        ServerConnection.Reset();
    }

    public void Dispose()
    {
        ServerConnection.Dispose();
    }
}