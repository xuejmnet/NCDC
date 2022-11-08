using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.ServerHandlers.Results;

namespace NCDC.ProxyServer.ServerHandlers;

public sealed class SkipServerHandler:IServerHandler
{
    public static SkipServerHandler Default { get; } = new SkipServerHandler();
    public Task<IServerResult> ExecuteAsync()
    {
        return Task.FromResult((IServerResult)RecordsAffectedServerResult.Default);
    }
}