using System.Text;
using DotNetty.Transport.Channels;
using NCDC.ProxyServer;
using NCDC.ProxyServer.Extensions;

namespace NCDC.ProxyClient.DotNetty;

public sealed class ChannelAttrInitializer:ChannelHandlerAdapter
{
    public override void ChannelActive(IChannelHandlerContext context)
    {
        context.Channel.SetEncoding(Encoding.UTF8);
        context.FireChannelActive();
    }
}