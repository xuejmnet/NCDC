using DotNetty.Transport.Channels;
using NCDC.Protocol.Packets;

namespace NCDC.ProxyClient.Authentication;

public interface IAuthenticationEngine
{
    int Handshake(IChannelHandlerContext context);
    AuthenticationResult Authenticate(IChannelHandlerContext context, IPacketPayload payload);
}