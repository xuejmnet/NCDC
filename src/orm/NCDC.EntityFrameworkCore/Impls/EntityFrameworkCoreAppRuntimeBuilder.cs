using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NCDC.Basic.Configurations;
using NCDC.EntityFrameworkCore.Entities;
using NCDC.Exceptions;
using NCDC.ProxyServer.AppServices.Abstractions;
using NCDC.ProxyServer.Runtimes.Builder;

namespace NCDC.EntityFrameworkCore.Impls;

public sealed class EntityFrameworkCoreAppRuntimeBuilder : AbstractAppRuntimeBuilder
{
    private readonly IServiceProvider _appServiceProvider;
    private LogicDatabaseEntity? _logicDatabase;
    private readonly List<DataSourceNode> _dataSourceNodes = new();
    private readonly List<LogicTableNode> _logicTableNodes = new();
    private readonly Dictionary<string, List<ActualTableNode>> _actualTableNodes = new();

    public EntityFrameworkCoreAppRuntimeBuilder(IServiceProvider appServiceProvider, IAppConfiguration appConfiguration)
        : base(appServiceProvider, appConfiguration)
    {
        _appServiceProvider = appServiceProvider;
    }

    protected override async Task LoadConfigurationAsync(string database)
    {
        using (var scope = _appServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<NCDCDbContext>();
            var logicDatabase = await dbContext.Set<LogicDatabaseEntity>().FirstOrDefaultAsync(o => o.Name == database);

            _logicDatabase = logicDatabase ?? throw new ShardingConfigException($"database: {database} not found");

            var dataSources = await dbContext.Set<DataSourceEntity>().Where(o => o.Database == database).ToListAsync();
            var logicTables = await dbContext.Set<LogicTableEntity>().Where(o => o.Database == database).ToListAsync();
            var actualTables =
                await dbContext.Set<ActualTableEntity>().Where(o => o.Database == database).ToListAsync();
            _dataSourceNodes.AddRange(dataSources.Select(o =>
                new DataSourceNode(o.Name, o.ConnectionString, o.IsDefault)));
            _logicTableNodes.AddRange(logicTables.Select(o =>
                new LogicTableNode(o.TableName, o.ShardingDataSourceRule, o.ShardingTableRule)));
            var map = actualTables.GroupBy(o => o.LogicTableName).ToDictionary(o => o.Key,
                o => o.Select(t => new ActualTableNode(t.DataSource, t.TableName)).ToList());
            foreach (var kv in map)
            {
                _actualTableNodes.Add(kv.Key, kv.Value);
            }
        }
    }

    protected override void ConfigureOption(ShardingConfiguration shardingConfiguration)
    {
        if (_logicDatabase == null)
        {
            throw new InvalidOperationException($"should {nameof(LoadConfigurationAsync)} first");
        }

        shardingConfiguration.AutoUseWriteConnectionStringAfterWriteDb =
            _logicDatabase.AutoUseWriteConnectionStringAfterWriteDb;
        shardingConfiguration.ThrowIfQueryRouteNotMatch = _logicDatabase.ThrowIfQueryRouteNotMatch;
        shardingConfiguration.MaxQueryConnectionsLimit = _logicDatabase.MaxQueryConnectionsLimit;
        shardingConfiguration.ConnectionMode = _logicDatabase.ConnectionMode;
    }

    protected override IEnumerable<DataSourceNode> GetDataSources()
    {
        foreach (var dataSourceNode in _dataSourceNodes)
        {
            yield return dataSourceNode;
        }
    }

    protected override IEnumerable<LogicTableNode> GetLogicTables()
    {
        foreach (var logicTableNode in _logicTableNodes)
        {
            yield return logicTableNode;
        }
    }

    protected override List<ActualTableNode> GetActualTables(string logicTableName)
    {
        if (_actualTableNodes.TryGetValue(logicTableName, out var actualTableNodes))
        {
            return actualTableNodes;
        }

        return new List<ActualTableNode>(0);
    }
}