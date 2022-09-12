using Microsoft.Extensions.DependencyInjection;
using NCDC.Basic.Metadatas;
using NCDC.Basic.TableMetadataManagers;
using NCDC.ProxyServer.Executors;

namespace NCDC.ProxyServer.Contexts;

public sealed class ShardingRuntimeContext:IRuntimeContext
{
    
    private bool isInited = false;
    private object INIT_LOCK = new object();
    private bool isInitModeled = false;
    private object INIT_MODEL = new object();
    private bool isCheckRequirement = false;
    private object CHECK_REQUIREMENT = new object();
    public  IServiceCollection Services { get; }

    private IServiceProvider _serviceProvider;
    
    public string DatabaseName { get; }

    public ShardingRuntimeContext(string databaseName)
    {
        DatabaseName = databaseName;
        Services= new ServiceCollection();
    }
    
    private void CheckIfBuild()
    {
        if (isInited)
            throw new InvalidOperationException("sharding runtime already build");
    }
    private void CheckIfNotBuild()
    {
        if (!isInited)
            throw new InvalidOperationException("sharding runtime not init");
    }

    private ILogicDatabase? _logicDatabase;
    public ILogicDatabase GetDatabase()
    {
        return _logicDatabase??=GetRequiredService<ILogicDatabase>();
    }

    private ITableMetadataManager? _tableMetadataManager;
    public ITableMetadataManager GetTableMetadataManager()
    {
      return _tableMetadataManager??=GetRequiredService<ITableMetadataManager>();
    }

    private IShardingExecutionContextFactory? _shardingExecutionContextFactory;
    public IShardingExecutionContextFactory GetShardingExecutionContextFactory()
    {
        return _shardingExecutionContextFactory??=GetRequiredService<IShardingExecutionContextFactory>();
    }

    public void Build()
    {
        if (isInited)
            return;

        lock (INIT_LOCK)
        {
            if (isInited)
                return;
            isInited = true;
            _serviceProvider = Services.BuildServiceProvider();
            // _serviceProvider.GetRequiredService<IShardingInitializer>().Build();
            InitFieldValue();
        }
    }

    public object? GetService(Type serviceType)
    {
        CheckIfNotBuild();
        return _serviceProvider.GetService(serviceType);
    }

    public TService? GetService<TService>()
    {
        CheckIfNotBuild();
        return _serviceProvider.GetService<TService>();
    }

    public object GetRequiredService(Type serviceType)
    {
        CheckIfNotBuild();
        return _serviceProvider.GetRequiredService(serviceType);
    }

    public TService GetRequiredService<TService>() where TService : notnull
    {
        CheckIfNotBuild();
        return _serviceProvider.GetRequiredService<TService>();
    }

    public object CreateInstance(Type serviceType,  params object[] parameters)
    {
        return ActivatorUtilities.CreateInstance(_serviceProvider,serviceType, parameters);
    }

    public TService CreateInstance<TService>(params object[] parameters)
    {
        return ActivatorUtilities.CreateInstance<TService>(_serviceProvider, parameters);
    }

    private void InitFieldValue()
    {
        GetDatabase();
        GetTableMetadataManager();
        GetShardingExecutionContextFactory();
    }
}