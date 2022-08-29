using OpenConnector.ProxyServer.Abstractions;
using OpenConnector.ProxyServer.ServerHandlers.Results;

namespace OpenConnector.ProxyServer.ServerHandlers;

public sealed class SkipServerHandler:IServerHandler
{
    public IServerResult Execute()
    {
        return new RecordsAffectedServerResult();
    }
}