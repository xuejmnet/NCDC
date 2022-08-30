using OpenConnector.ProxyServer.Abstractions;

namespace OpenConnector.ProxyClient.Command;

public class DefaultCommandListener : ICommandListener
{

    public event ReceivedDelegate? Received;

    public ValueTask OnReceived(ICommand messageSender)
    {
        if (Received != null)
            return Received(messageSender);
        return ValueTask.CompletedTask;
    }
}