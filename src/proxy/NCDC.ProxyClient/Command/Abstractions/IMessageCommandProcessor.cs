using DotNetty.Transport.Channels;
using NCDC.ProxyServer.Abstractions;

namespace NCDC.ProxyClient.Command.Abstractions;

public interface IMessageCommandProcessor
{
    IMessageExecutor GetMessageExecutor(IChannelId channelId);
    bool TryReceived(IChannelId channelId,ICommand command);
    void Register(IChannelId channelId);
    void UnRegister(IChannelId channelId);
}