using NCDC.Base;
using NCDC.Enums;
using NCDC.Exceptions;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.Connection;

public sealed class ServerConnection : IServerConnection
{
    private const int MAXIMUM_RETRY_COUNT = 5;

    private readonly OneByOneChecker _oneByOne = new OneByOneChecker();

    private TransactionTypeEnum _transactionType;
    private bool _supportHint;
    public int ConnectionId { get; set; }
    public string UserName { get; set; }

    public IDictionary<string/*data source*/, List<IServerDbConnection>> CachedConnections { get; } =
        new Dictionary<string, List<IServerDbConnection>>();

    public ServerConnection(IConnectionSession connectionSession)
    {
        _transactionType = TransactionTypeEnum.LOCAL;
        ConnectionSession = connectionSession;
        ServerDbConnectionInvokeReplier = new LinkedList<Func<IServerDbConnection,Task>>();
    }

    public IConnectionSession ConnectionSession { get; }

    public async ValueTask<List<IServerDbConnection>> GetConnections(ConnectionModeEnum connectionMode, string dataSourceName,
        int connectionSize)
    {
        OneByOneLock();
        try
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
                var serverDbConnections =await GetServerDbConnectionFromContextAsync(connectionMode, dataSourceName,
                    connectionSize - cachedConnections.Count());
                dbConnections.AddRange(serverDbConnections);
                cachedConnections.AddRange(serverDbConnections);
                return dbConnections;
            }
        }
        finally
        {
            OneByOneUnLock();
        }
    }

    public async ValueTask<LinkedList<Exception>> ReleaseConnectionsAsync(bool forceRollback)
    {
        var result = new LinkedList<Exception>();
        foreach (var connections in CachedConnections.Values)
        {
            foreach (var serverDbConnection in connections)
            {
                try
                {
                    if (forceRollback && ConnectionSession.GetTransactionStatus().IsInTransaction())
                    {
                        await serverDbConnection.RollbackAsync();
                    }

                    serverDbConnection.Dispose();
                }
                catch (Exception ex)
                {
                    result.AddLast(ex);
                }
            }
        }

        CachedConnections.Clear();
        ServerDbConnectionInvokeReplier.Clear();
        return result;
    }

    private async Task<List<IServerDbConnection>> GetServerDbConnectionFromContextAsync(ConnectionModeEnum connectionMode,
        string dataSourceName, int connectionSize)
    {
        if (ConnectionSession.VirtualDataSource == null)
        {
            throw new ArgumentException("current database is null");
        }

        var serverDbConnections = await ConnectionSession.VirtualDataSource.GetServerDbConnectionsAsync(connectionMode, dataSourceName,
            connectionSize,
            _transactionType);
        foreach (var serverDbConnection in serverDbConnections)
        {
            IAdoMethodReplier adoMethodReplier = this;
           await adoMethodReplier.ReplyTargetMethodInvokeAsync(serverDbConnection);
        }
        return serverDbConnections;
    }

    public LinkedList<Func<IServerDbConnection,Task>> ServerDbConnectionInvokeReplier { get; }

    private void OneByOneLock()
    {
        //是否触发并发了
        var acquired = _oneByOne.Start();
        if (!acquired)
        {
            throw new ShardingException($"{nameof(OneByOneLock)} cant parallel use in connection ");
        }
    }

    private void OneByOneUnLock()
    {
        _oneByOne.Stop();
    }

    public void Dispose()
    {
        CachedConnections.Clear();
    }

    public IServerDbConnection GetServerDbConnection(CreateServerDbConnectionStrategyEnum strategy, string dataSourceName)
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        CachedConnections.Clear();
    }
}