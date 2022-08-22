using DotNetty.Transport.Channels;
using ShardingConnector.Protocol.Packets;

namespace ShardingConnector.ProxyClient.Authentication;

public interface IAuthenticationEngine
{
    int Handshake(IChannelHandlerContext context);
    AuthenticationResult Authenticate(IChannelHandlerContext context, IPacketPayload payload);
}