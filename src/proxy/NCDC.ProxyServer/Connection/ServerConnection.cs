using System.Collections.Concurrent;
using System.Collections.Immutable;
using NCDC.Base;
using NCDC.Enums;
using NCDC.Exceptions;
using NCDC.ProxyServer.Connection.Abstractions;
using Nito.AsyncEx;

namespace NCDC.ProxyServer.Connection;

public sealed class ServerConnection : IServerConnection
{
    private const int MAXIMUM_RETRY_COUNT = 5;
    private static readonly object slock = new object();
    private readonly AsyncLock _mutex = new AsyncLock();


    private TransactionTypeEnum _transactionType;
    private bool _supportHint;
    public int ConnectionId { get; set; }
    public string UserName { get; set; }

    public IDictionary<string /*data source*/, List<IServerDbConnection>> CachedConnections { get; } =
        new ConcurrentDictionary<string, List<IServerDbConnection>>();

    // private IDictionary<string, OneByOneChecker> _checkers;

    public ServerConnection(IConnectionSession connectionSession)
    {
        _transactionType = TransactionTypeEnum.LOCAL;
        ConnectionSession = connectionSession;
        ServerDbConnectionInvokeReplier = new LinkedList<Func<IServerDbConnection, Task>>();
        // var allDataSourceNames = connectionSession.VirtualDataSource!.GetAllDataSourceNames();
        // _checkers=allDataSourceNames.ToImmutableDictionary(o => o, o => new OneByOneChecker());
    }

    public IConnectionSession ConnectionSession { get; }

    /// <summary>
    /// 获取ado链接用来和数据库交互
    /// </summary>
    /// <param name="connectionMode"></param>
    /// <param name="dataSourceName"></param>
    /// <param name="connectionSize"></param>
    /// <returns></returns>
    public async ValueTask<List<IServerDbConnection>> GetConnections(ConnectionModeEnum connectionMode,
        string dataSourceName,
        int connectionSize)
    {
        // if (!_checkers.TryGetValue(dataSourceName, out var checker))
        // {
        //     throw new InvalidOperationException($"cant found:{dataSourceName} checker");
        // }
        // checker.OneByOneLock();
        using (await _mutex.LockAsync())
        {
         
            if (!CachedConnections.TryGetValue(dataSourceName, out var cachedConnections))
            {
                cachedConnections = new List<IServerDbConnection>(Math.Max(connectionSize * 2,
                    Environment.ProcessorCount));
                CachedConnections.TryAdd(dataSourceName, cachedConnections);
            }

            if (cachedConnections.Count >= connectionSize)
            {
                return cachedConnections.Take(connectionSize).ToList();
            }
            else
            {
                var dbConnections = new List<IServerDbConnection>(connectionSize);
                dbConnections.AddRange(cachedConnections);
                var serverDbConnections = await GetServerDbConnectionFromContextAsync(connectionMode, dataSourceName,
                    connectionSize - cachedConnections.Count());
                dbConnections.AddRange(serverDbConnections);
                cachedConnections.AddRange(serverDbConnections);
                return dbConnections;
            }   
        }
    }

    public async ValueTask ReleaseConnectionsAsync(bool forceRollback)
    {
        var releaseConnections = CachedConnections.Values
            .SelectMany(connections => connections.Select(serverDbConnection =>
                ReleaseServerDbConnectionAsync(forceRollback, serverDbConnection))).ToArray();
        try
        {
            await Task.WhenAll(releaseConnections);
        }
        finally
        {
            CachedConnections.Clear();
            ServerDbConnectionInvokeReplier.Clear();
        }
    }

    private async Task ReleaseServerDbConnectionAsync(bool forceRollback, IServerDbConnection serverDbConnection)
    {
        if (forceRollback && ConnectionSession.GetTransactionStatus().IsInTransaction())
            await serverDbConnection.RollbackAsync();
        serverDbConnection.Dispose();
    }

    private async Task<List<IServerDbConnection>> GetServerDbConnectionFromContextAsync(
        ConnectionModeEnum connectionMode,
        string dataSourceName, int connectionSize)
    {
        if (ConnectionSession.VirtualDataSource == null)
        {
            throw new ArgumentException("current database is null");
        }

        var serverDbConnections = await ConnectionSession.VirtualDataSource.GetServerDbConnectionsAsync(connectionMode,
            dataSourceName,
            connectionSize,
            _transactionType);
        foreach (var serverDbConnection in serverDbConnections)
        {
            IAdoMethodReplier adoMethodReplier = this;
            await adoMethodReplier.ReplyTargetMethodInvokeAsync(serverDbConnection);
        }

        return serverDbConnections;
    }

    public LinkedList<Func<IServerDbConnection, Task>> ServerDbConnectionInvokeReplier { get; }


    public void Dispose()
    {
        CachedConnections.Clear();
    }

    public IServerDbConnection GetServerDbConnection(CreateServerDbConnectionStrategyEnum strategy,
        string dataSourceName)
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        CachedConnections.Clear();
        ServerDbConnectionInvokeReplier.Clear();
    }
}