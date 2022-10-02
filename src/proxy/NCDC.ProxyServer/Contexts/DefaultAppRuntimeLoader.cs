using NCDC.ProxyServer.Configurations.Apps;

namespace NCDC.ProxyServer.Contexts;

public sealed class DefaultAppRuntimeLoader:IAppRuntimeLoader
{
    private readonly IContextManager _contextManager;

    public DefaultAppRuntimeLoader(IContextManager contextManager)
    {
        _contextManager = contextManager;
    }
    public bool HasLoaded(string databaseName)
    {
        return _contextManager.HasRuntimeContext(databaseName);
    }

    public bool Load(IRuntimeContext runtimeContext)
    {
        if (HasLoaded(runtimeContext.DatabaseName))
        {
            return false;
        }

        return _contextManager.AddRuntimeContext(runtimeContext);
    }

    public bool UnLoad(string databaseName)
    {
        if (!HasLoaded(databaseName))
        {
            return false;
        }

        return _contextManager.RemoveRuntimeContext(databaseName, out _);
    }
}