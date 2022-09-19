using DotNetty.Transport.Channels;
using NCDC.Protocol.Packets;

namespace NCDC.ProxyClient.Authentication;

public interface IAuthenticationHandler
{
    int Handshake(IChannelHandlerContext context,IAuthContext authContext);
    AuthenticationResult Authenticate(IChannelHandlerContext context, IPacketPayload payload,IAuthContext authContext);
}
public interface IAuthenticationHandler<TPacketPayload,TContext>:IAuthenticationHandler where TPacketPayload:IPacketPayload
where TContext:IAuthContext
{
    int Handshake(IChannelHandlerContext context, TContext authContext);

    int IAuthenticationHandler.Handshake(IChannelHandlerContext context, IAuthContext authContext)
    {
        return Handshake(context, (TContext)authContext);
    }
    AuthenticationResult Authenticate(IChannelHandlerContext context, TPacketPayload payload,TContext authContext);

    AuthenticationResult IAuthenticationHandler.Authenticate(IChannelHandlerContext context, IPacketPayload payload,
        IAuthContext authContext)
    {
         return Authenticate(context, (TPacketPayload)payload, (TContext)authContext);
    }
}