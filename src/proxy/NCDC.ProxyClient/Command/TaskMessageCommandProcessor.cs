using System.Collections.Concurrent;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using NCDC.Logger;
using NCDC.ProxyClient.Command.Abstractions;
using NCDC.ProxyServer.Abstractions;

namespace NCDC.ProxyClient.Command;

public sealed class TaskMessageCommandProcessor:IMessageCommandProcessor
{

    private static readonly ILogger<TaskMessageCommandProcessor> _logger =
        NCDCLoggerFactory.CreateLogger<TaskMessageCommandProcessor>();
    
    private readonly ConcurrentDictionary<IChannelId, IMessageExecutor> _executors = new();
    private readonly IMessageExecutorFactory _messageExecutorFactory;

    public TaskMessageCommandProcessor(IMessageExecutorFactory messageExecutorFactory)
    {
        _messageExecutorFactory = messageExecutorFactory;
    }

    public IMessageExecutor GetMessageExecutor(IChannelId channelId)
    {
        if (!_executors.TryGetValue(channelId, out var messageExecutor))
        {
            throw new InvalidOperationException(
                $"cant get {nameof(IMessageExecutor)} channel id:[{channelId.AsShortText()}]");
        }

        return messageExecutor;
    }

    public bool TryReceived(IChannelId channelId, ICommand command)
    {
        if (!_executors.TryGetValue(channelId, out var messageExecutor))
        {
            throw new InvalidOperationException(
                $"cant get {nameof(IMessageExecutor)} channel id:[{channelId.AsShortText()}]");
        }

        return messageExecutor.TryAddMessage(command);
    }

    public IMessageExecutor Register(IChannelId channelId)
    {
        var messageExecutor = _messageExecutorFactory.Create();
        if (_executors.TryAdd(channelId, messageExecutor))
        {
            return messageExecutor;
        }

        throw new InvalidOperationException($"repeat register message executor:[{channelId.AsShortText()}]");
    }

    public void UnRegister(IChannelId channelId)
    {
        if (_executors.Remove(channelId, out var executor))
        {
            try
            {
                executor.Dispose();
            }
            catch (Exception e)
            {
                _logger.LogError($"UnRegister error,channel id:[{channelId.AsShortText()}],error:{e}.");
            }
        }
    }
}