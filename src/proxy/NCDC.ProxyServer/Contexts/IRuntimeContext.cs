using NCDC.Basic.Metadatas;
using NCDC.Basic.TableMetadataManagers;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.Metadatas;
using NCDC.ProxyServer.Executors;
using NCDC.ShardingMerge.Abstractions;

namespace NCDC.ProxyServer.Contexts;

public interface IRuntimeContext
{
    string DatabaseName { get; }
    ILogicDatabase GetDatabase();
    ITableMetadataManager GetTableMetadataManager();
    IShardingExecutionContextFactory GetShardingExecutionContextFactory();
    IDataReaderMergerFactory GetDataReaderMergerFactory();
    
    void Build();
    object? GetService(Type serviceType);
    TService? GetService<TService>();
    object GetRequiredService(Type serviceType);
    TService GetRequiredService<TService>() where TService : notnull;
    object CreateInstance(Type serviceType, params object[] parameters);
    TService CreateInstance<TService>(params object[] parameters);
}