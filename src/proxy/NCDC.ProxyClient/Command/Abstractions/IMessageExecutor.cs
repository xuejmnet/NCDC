using NCDC.ProxyServer.Abstractions;

namespace NCDC.ProxyClient.Command.Abstractions;

public interface IMessageExecutor:IDisposable
{
    bool TryAddMessage(ICommand command);
}