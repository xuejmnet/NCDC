using DotNetty.Transport.Channels;
using ShardingConnector.Proxy.Network.Servers;

namespace ShardingConnector.Proxy.Network.Authentications;

public class MySqlAuthenticationEngine:IAuthenticationEngine
{
    public bool Auth(IChannelHandlerContext context, IPacketPayload payload, ServerConnection serverConnection)
    {
        throw new NotImplementedException();
    }
}