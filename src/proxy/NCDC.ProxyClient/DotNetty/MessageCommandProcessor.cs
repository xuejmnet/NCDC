using System.Collections.Concurrent;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using NCDC.Logger;
using NCDC.ProxyClient.Command;

namespace NCDC.ProxyClient.DotNetty;

public class MessageCommandProcessor
{
    private static readonly ILogger<MessageCommandProcessor> _logger =
        InternalNCDCLoggerFactory.CreateLogger<MessageCommandProcessor>();
    public static MessageCommandProcessor Instance { get; } = new();
    private ConcurrentDictionary<IChannelId, IMessageExecutor> _executors = new();

    public void Register(IChannelId channelId)
    {
        _executors.TryAdd(channelId, new MessageExecutor());
    }

    public IMessageExecutor Get(IChannelId channelId)
    {
        if (!_executors.TryGetValue(channelId, out var messageExecutor))
        {
            throw new InvalidOperationException(
                $"cant get {nameof(IMessageExecutor)} channel id:[{channelId.AsShortText()}]");
        }

        return messageExecutor;
    }
    public void UnRegister(IChannelId channelId) {
        if (_executors.Remove(channelId, out var executor))
        {
            try
            {
                executor.Dispose();
            }
            catch (Exception e)
            {
                _logger.LogError($"UnRegister error,channel id:[{channelId.AsShortText()}].",e);
            }
        }
    }
}