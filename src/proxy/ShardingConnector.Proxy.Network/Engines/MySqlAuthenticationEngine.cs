using DotNetty.Transport.Channels;
using ShardingConnector.Proxy.Network.Packets.Handshakes;
using ShardingConnector.Proxy.Network.Servers;

namespace ShardingConnector.Proxy.Network.Engines;

public class MySqlAuthenticationEngine:IAuthenticationEngine
{
    public void Handshake(IChannelHandlerContext context, ServerConnection serverConnection)
    {
        serverConnection.ConnectionId = 1;
        var mySqlHandshakePacket = new MySqlHandshakePacket(1, new MySqlAuthPluginData());
        context.WriteAndFlushAsync(mySqlHandshakePacket).GetAwaiter().GetResult();
    }
}