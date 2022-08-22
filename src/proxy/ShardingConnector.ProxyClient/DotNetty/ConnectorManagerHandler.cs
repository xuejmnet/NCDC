using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using ShardingConnector.Logger;

namespace ShardingConnector.ProxyClient.DotNetty;

public class ConnectorManagerHandler : ChannelDuplexHandler
{
    private static readonly ILogger<ConnectorManagerHandler> _logger =
        InternalLoggerFactory.CreateLogger<ConnectorManagerHandler>();

    private readonly bool _logDebug;

    public ConnectorManagerHandler()
    {
        _logDebug = _logger.IsEnabled(LogLevel.Debug);
    }

    public override bool IsSharable => true;

    public override void ChannelRegistered(IChannelHandlerContext context)
    {
        if (_logDebug)
        {
            string remoteAddress = RemotingHelper.ParseChannelRemoteAddress(context.Channel);
            _logger.LogDebug($"NETTY SERVER PIPELINE: ChannelRegistered {remoteAddress}");
        }

        base.ChannelRegistered(context);
    }

    public override void ChannelUnregistered(IChannelHandlerContext context)
    {
        if (_logDebug)
        {
            string remoteAddress = RemotingHelper.ParseChannelRemoteAddress(context.Channel);
            _logger.LogDebug($"NETTY SERVER PIPELINE: ChannelUnregistered, the channel[{remoteAddress}]");
        }

        base.ChannelUnregistered(context);
    }

    public override void ChannelActive(IChannelHandlerContext context)
    {
        if (_logDebug)
        {
            string remoteAddress = RemotingHelper.ParseChannelRemoteAddress(context.Channel);
            _logger.LogDebug($"NETTY SERVER PIPELINE: ChannelActive, the channel[{remoteAddress}]");
        }

        base.ChannelActive(context);
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        if (_logDebug)
        {
            string remoteAddress = RemotingHelper.ParseChannelRemoteAddress(context.Channel);
            _logger.LogDebug($"NETTY SERVER PIPELINE: channelInactive, the channel[{remoteAddress}]");
        }

        base.ChannelInactive(context);
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        if (_logDebug)
        {
            string remoteAddress = RemotingHelper.ParseChannelRemoteAddress(context.Channel);
            _logger.LogError($"NETTY SERVER PIPELINE: exceptionCaught {remoteAddress}");
            _logger.LogError("NETTY SERVER PIPELINE: exceptionCaught exception.", exception);
        }
        base.ExceptionCaught(context,exception);
    }
}

