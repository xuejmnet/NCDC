using NCDC.Basic.Metadatas;
using NCDC.Basic.TableMetadataManagers;
using NCDC.CommandParser.Abstractions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.Metadatas;
using NCDC.ProxyServer.Executors;
using NCDC.ProxyServer.ServiceProviders;
using NCDC.ShardingMerge.Abstractions;
using NCDC.ShardingRoute.Abstractions;

namespace NCDC.ProxyServer.Contexts;

public interface IRuntimeContext
{
    string DatabaseName { get; }
    ILogicDatabase GetDatabase();
    ITableMetadataManager GetTableMetadataManager();
    IDataSourceRouteManager GetDataSourceRouteManager();
    ITableRouteManager GetTableRouteManager();
    IShardingExecutionContextFactory GetShardingExecutionContextFactory();
    IDataReaderMergerFactory GetDataReaderMergerFactory();
    ISqlCommandParser GetSqlCommandParser();
    IShardingProvider GetShardingProvider();
    object? GetService(Type serviceType);
    TService? GetService<TService>();
    object GetRequiredService(Type serviceType);
    TService GetRequiredService<TService>() where TService : notnull;
    object CreateInstance(Type serviceType, params object[] parameters);
    TService CreateInstance<TService>(params object[] parameters);
}