namespace OpenConnector.ProxyServer.Abstractions;

public delegate ValueTask ReceivedDelegate(ICommand messageSender);
public interface ICommandListener
{
    /// <summary>
    /// 接收到的消息
    /// </summary>
    event ReceivedDelegate Received;

    ValueTask OnReceived(ICommand messageSender);
}