using DotNetty.Transport.Channels;
using ShardingConnector.ProtocolCore.Payloads;
using ShardingConnector.ProxyServer.Connection;

namespace ShardingConnector.ProxyClient.Authentication;

public interface IAuthenticationEngine
{
    int Handshake(IChannelHandlerContext context);
    AuthenticationResult Authenticate(IChannelHandlerContext context, IPacketPayload payload);
}