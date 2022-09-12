using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.ServerHandlers.Results;

namespace NCDC.ProxyServer.ServerHandlers;

public sealed class SkipServerHandler:IServerHandler
{
    public IServerResult Execute()
    {
        return new RecordsAffectedServerResult();
    }
}