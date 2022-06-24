using DotNetty.Transport.Channels;
using ShardingConnector.Proxy.Network.Servers;

namespace ShardingConnector.Proxy.Network.Authentications;

public interface IAuthenticationEngine
{
    void Handshake(IChannelHandlerContext context, ServerConnection serverConnection);
    bool Auth(IChannelHandlerContext context, IPacketPayload payload, ServerConnection serverConnection);
}