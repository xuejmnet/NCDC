using System.Text;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using ShardingConnector.ProtocolCore;
using ShardingConnector.ProtocolMysql.Constant;
using ShardingConnector.ProtocolMysql.Packet.Generic;
using ShardingConnector.ProxyServer.Connection;
using ShardingConnector.ProxyServer.Session;
using ShardingConnector.ShardingCommon.User;
using ShardingConnector.Transaction;

namespace ShardingConnector.ProxyClient.DotNetty;

public class ClientChannelInboundHandler:SimpleChannelInboundHandler<object>
{
    private readonly IDatabaseProtocolClientEngine _databaseProtocolClientEngine;
    private readonly ISocketChannel _channel;
    private readonly ServerConnection _serverConnection = new ServerConnection(TransactionTypeEnum.LOCAL);

    private readonly ConnectionSession _connectionSession;
    // private readonly IAuthenticationEngine _authenticationEngine = new MySqlAuthenticationEngine();

    private  volatile bool _authenticated;
    public ClientChannelInboundHandler(IDatabaseProtocolClientEngine databaseProtocolClientEngine,ISocketChannel channel)
    {
        _databaseProtocolClientEngine = databaseProtocolClientEngine;
        _channel = channel;
        _connectionSession = new ConnectionSession(TransactionTypeEnum.LOCAL,channel);
    }
    public override void ChannelActive(IChannelHandlerContext context)
    {
        var connectionId = _databaseProtocolClientEngine.GetAuthenticationEngine().Handshake(context);
        _connectionSession.SetConnectionId(connectionId);
    }


    protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
    {
        var byteBuffer = (IByteBuffer)msg;
        var s = byteBuffer.ToString(Encoding.UTF8);
        Console.WriteLine(s);
        byteBuffer.Retain();
        Console.WriteLine("收到消息");
        if (!_authenticated)
        {
            Console.WriteLine("authorized:"+_authenticated);
            _authenticated = Authenticate(ctx, byteBuffer);
            return;
        }

        Console.WriteLine("认证："+_authenticated);
        // var appCommand = new AppCommand(this._serverConnection,ctx,msg);
        // Task.Run(async () =>
        // {
        //     await appCommand.ExecuteAsync();
        // });

        // var byteBuffer = (IByteBuffer)msg;
        // var s = byteBuffer.ToString(Encoding.Default);
        // Console.WriteLine(s);
        // byteBuffer.Retain();
    }

    private bool Authenticate(IChannelHandlerContext context,IByteBuffer message)
    {
        using (var payload = _databaseProtocolClientEngine.GetCodecEngine().CreatePacketPayload(message,context.GetAttribute(CommonConstants.CHARSET_ATTRIBUTE_KEY).Get()))
        {
            try
            {
                var authenticationResult = _databaseProtocolClientEngine.GetAuthenticationEngine().Authenticate(context,payload);
                if (authenticationResult.Finished)
                {
                    _connectionSession.SetGrantee(new Grantee(authenticationResult.Username,authenticationResult.Hostname));
                }

                return authenticationResult.Finished;
            }
            catch (Exception e)
            {
                Console.WriteLine("exception occur:");
                Console.WriteLine($"{e}");
                context.WriteAndFlushAsync(new MySqlErrPacket(1, MySqlServerErrorCode.ER_NO_DB_ERROR));
            }
        }

        return false;
    }
}