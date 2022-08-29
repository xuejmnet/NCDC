using DotNetty.Transport.Channels;
using OpenConnector.Protocol.Packets;

namespace OpenConnector.ProxyClient.Authentication;

public interface IAuthenticationEngine
{
    int Handshake(IChannelHandlerContext context);
    AuthenticationResult Authenticate(IChannelHandlerContext context, IPacketPayload payload);
}