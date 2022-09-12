using NCDC.Basic.Executors;
using NCDC.Basic.Metadatas;
using NCDC.Basic.TableMetadataManagers;

namespace NCDC.Basic.Contexts;

public interface IRuntimeContext
{
    string DatabaseName { get; }
    ILogicDatabase GetDatabase();
    ITableMetadataManager GetTableMetadataManager();
    IShardingExecutionContextFactory GetShardingExecutionContextFactory();
    
    
    void Build();
    object? GetService(Type serviceType);
    TService? GetService<TService>();
    object GetRequiredService(Type serviceType);
    TService GetRequiredService<TService>() where TService : notnull;
    object CreateInstance(Type serviceType, params object[] parameters);
    TService CreateInstance<TService>(params object[] parameters);
}