using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NCDC.Basic.Configurations;
using NCDC.Basic.TableMetadataManagers;
using NCDC.EntityFrameworkCore.Entities;
using NCDC.ProxyServer.Databases;
using NCDC.ProxyServer.Options;
using NCDC.ProxyServer.Runtimes.Initializer;
using NCDC.ProxyServer.ServiceProviders;

namespace NCDC.EntityFrameworkCore.Impls;

public sealed class EntityFrameworkCoreRuntimeInitializer:AbstractRuntimeInitializer
{
    private readonly IShardingProvider _shardingProvider;
    private readonly IVirtualDataSource _virtualDataSource;

    private readonly IDictionary<string, List<ActualTableNode>> _actualTableNodeMap =
        new Dictionary<string, List<ActualTableNode>>();

    public EntityFrameworkCoreRuntimeInitializer(IShardingProvider shardingProvider, ShardingConfiguration shardingConfiguration, ITableMetadataManager tableMetadataManager, IVirtualDataSource virtualDataSource, IRouteInitConfigOption routeInitConfigOption, ITableMetadataInitializer tableMetadataInitializer) : base(shardingProvider, shardingConfiguration, tableMetadataManager, virtualDataSource, routeInitConfigOption, tableMetadataInitializer)
    {
        _shardingProvider = shardingProvider;
        _virtualDataSource = virtualDataSource;
    }

    protected override async Task LoadTableSchemaAsync()
    {
        
        using (var serviceScope = _shardingProvider.ApplicationServiceProvider.CreateScope())
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<NCDCDbContext>();
            var dbActualTables = await dbContext.Set<ActualTableEntity>().Where(o=>o.Database==_virtualDataSource.GetDatabaseName()).ToListAsync();
            var map = dbActualTables.GroupBy(o=>o.LogicTableName).ToDictionary(o=>o.Key,o=>o.Select(t=>new ActualTableNode(t.DataSource,t.TableName)).ToList());
            foreach (var kv in map)
            {
                _actualTableNodeMap.Add(kv.Key,kv.Value);
            }
        }
    }

    protected override List<ActualTableNode> GetActualTables(string logicTableName)
    {
        if (_actualTableNodeMap.TryGetValue(logicTableName, out var actualTableNodes))
        {
            return actualTableNodes;
        }

        return new List<ActualTableNode>(0);
    }
}