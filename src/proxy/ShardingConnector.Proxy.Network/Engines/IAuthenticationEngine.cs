using DotNetty.Transport.Channels;
using ShardingConnector.Proxy.Network.Servers;

namespace ShardingConnector.Proxy.Network.Engines;

public interface IAuthenticationEngine
{
    void Handshake(IChannelHandlerContext context, ServerConnection serverConnection);
}