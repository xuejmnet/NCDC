using System.Text;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using ShardingConnector.Proxy.Application;
using ShardingConnector.Proxy.Network.Authentications;
using ShardingConnector.Proxy.Network.Servers;
using ShardingConnector.Transaction;

namespace ShardingConnector.Proxy.Network;

public class ApplicationChannelInboundHandler:SimpleChannelInboundHandler<object>
{
    private readonly ServerConnection _serverConnection = new ServerConnection(TransactionTypeEnum.LOCAL);
    private readonly IAuthenticationEngine _authenticationEngine = new MySqlAuthenticationEngine();

    private  volatile bool authorized;
    public ApplicationChannelInboundHandler()
    {
    }
    public override void ChannelActive(IChannelHandlerContext context)
    {
        _authenticationEngine.Handshake(context,_serverConnection);
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
        var byteBuffer = (IByteBuffer)msg;
        var s = byteBuffer.ToString(Encoding.UTF8);
        Console.WriteLine(s);
        byteBuffer.Retain();
        Console.WriteLine("收到消息");
        if (!authorized)
        {
            Console.WriteLine("authorized:"+authorized);
            authorized = Auth(ctx, byteBuffer);
            return;
        }

        Console.WriteLine("认证："+authorized);
        var appCommand = new AppCommand(this._serverConnection,ctx,msg);
        Task.Run(async () =>
        {
            await appCommand.ExecuteAsync();
        });

        // var byteBuffer = (IByteBuffer)msg;
        // var s = byteBuffer.ToString(Encoding.Default);
        // Console.WriteLine(s);
        // byteBuffer.Retain();
    }

    private bool Auth(IChannelHandlerContext context,IByteBuffer byteBuffer)
    {
        using (var payload = new MySqlPacketPayload(byteBuffer,Encoding.UTF8))
        {
            try
            {
                return _authenticationEngine.Auth(context, payload, _serverConnection);
            }
            catch (Exception e)
            {
                Console.WriteLine("exception occur:");
                Console.WriteLine($"{e}");
                // context.WriteAndFlushAsync()
            }
        }

        return false;
    }
}