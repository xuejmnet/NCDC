using System.Text;
using DotNetty.Transport.Channels;
using OpenConnector.ProxyServer;

namespace OpenConnector.ProxyClient.DotNetty;

public sealed class ChannelAttrInitializer:ChannelHandlerAdapter
{
    public override void ChannelActive(IChannelHandlerContext context)
    {
        context.Channel.GetAttribute(CommonConstants.CHARSET_ATTRIBUTE_KEY).SetIfAbsent(Encoding.UTF8);
        context.FireChannelActive();
    }
}