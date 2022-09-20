using NCDC.ProxyServer.Abstractions;

namespace NCDC.ProxyClient.Command;

public interface IMessageExecutor:IDisposable
{
    bool TryAddMessage(ICommand command);
}