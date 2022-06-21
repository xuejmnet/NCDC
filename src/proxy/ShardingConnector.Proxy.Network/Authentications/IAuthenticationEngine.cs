using DotNetty.Transport.Channels;
using ShardingConnector.Proxy.Network.Servers;

namespace ShardingConnector.Proxy.Network.Authentications;

public interface IAuthenticationEngine
{
    bool Auth(IChannelHandlerContext context, IPacketPayload payload, ServerConnection serverConnection);
}