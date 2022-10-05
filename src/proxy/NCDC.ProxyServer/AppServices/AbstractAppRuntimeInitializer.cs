using Microsoft.Extensions.Logging;
using NCDC.Logger;
using NCDC.ProxyServer.AppServices.Builder;

namespace NCDC.ProxyServer.AppServices;

public abstract class AbstractAppRuntimeInitializer:IAppRuntimeInitializer
{
    private readonly ILogger<AbstractAppRuntimeInitializer> _logger =
        InternalNCDCLoggerFactory.CreateLogger<AbstractAppRuntimeInitializer>();
    private readonly IAppRuntimeLoader _appRuntimeLoader;
    private readonly IAppRuntimeBuilder _appRuntimeBuilder;

    public AbstractAppRuntimeInitializer(IAppRuntimeLoader appRuntimeLoader,IAppRuntimeBuilder appRuntimeBuilder)
    {
        _appRuntimeLoader = appRuntimeLoader;
        _appRuntimeBuilder = appRuntimeBuilder;
    }
    public async Task InitializeAsync()
    {
        var runtimes = await GetRuntimesAsync();
        foreach (var database in runtimes)
        {
            var runtimeContext = await _appRuntimeBuilder.BuildAsync(database);
            if (!_appRuntimeLoader.LoadRuntimeContext(runtimeContext))
            {
                _logger.LogWarning($"repeat load runtime:{runtimeContext.DatabaseName}");
            }
        }
    }

    protected abstract Task<IReadOnlyCollection<string>> GetRuntimesAsync();
}