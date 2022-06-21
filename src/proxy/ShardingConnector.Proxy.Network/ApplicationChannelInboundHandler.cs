using System.Text;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using ShardingConnector.Proxy.Network.Engines;
using ShardingConnector.Proxy.Network.Servers;
using ShardingConnector.Transaction;

namespace ShardingConnector.Proxy.Network;

public class ApplicationChannelInboundHandler:SimpleChannelInboundHandler<object>
{
    private readonly ServerConnection _serverConnection = new ServerConnection(TransactionTypeEnum.LOCAL);

    private  volatile bool authorized;
    public ApplicationChannelInboundHandler()
    {
    }
    public override void ChannelActive(IChannelHandlerContext context)
    {
        var mySqlAuthenticationEngine = new MySqlAuthenticationEngine();
        mySqlAuthenticationEngine.Handshake(context,_serverConnection);
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        base.ChannelInactive(context);
    }

    public override void ChannelWritabilityChanged(IChannelHandlerContext context)
    {
        base.ChannelWritabilityChanged(context);
    }

    protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
    {
        if (!authorized)
        {
            authorized=a
        }
        Console.WriteLine("收到消息");

        var byteBuffer = (IByteBuffer)msg;
        var s = byteBuffer.ToString(Encoding.Default);
        Console.WriteLine(s);
        byteBuffer.Retain();
    }

    private bool Auth(IChannelHandlerContext context,IByteBuffer byteBuffer)
    {
        
    }
}