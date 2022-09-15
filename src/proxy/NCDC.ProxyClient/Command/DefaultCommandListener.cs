using NCDC.ProxyServer.Abstractions;

namespace NCDC.ProxyClient.Command;

public class DefaultCommandListener : ICommandListener
{

    public event ReceivedDelegate? OnReceived;

    public ValueTask Received(ICommand messageSender)
    {
        if (OnReceived != null)
            return OnReceived(messageSender);
        return ValueTask.CompletedTask;
    }
}