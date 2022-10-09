using System.Text;
using System.Threading.Channels;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Logging;
using NCDC.Basic.User;
using NCDC.Enums;
using NCDC.Logger;
using NCDC.Protocol.MySql.Constant;
using NCDC.Protocol.MySql.Packet.Generic;
using NCDC.ProxyClient.Authentication;
using NCDC.ProxyClient.Command;
using NCDC.ProxyClient.Command.Abstractions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.AppServices;
using NCDC.ProxyServer.AppServices.Abstractions;
using NCDC.ProxyServer.Connection;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.Extensions;
using NCDC.ProxyServer.Runtimes;

namespace NCDC.ProxyClient.DotNetty;

public class ClientChannelInboundHandler : ChannelHandlerAdapter
{
    private static readonly ILogger<ClientChannelInboundHandler> _logger =
        InternalNCDCLoggerFactory.CreateLogger<ClientChannelInboundHandler>();

    private readonly IDatabaseProtocolClientEngine _databaseProtocolClientEngine;
    private readonly ICommandListener _commandListener;
    private readonly IAppRuntimeManager _appRuntimeManager;
    private readonly IMessageCommandProcessor _messageCommandProcessor;

    private readonly IConnectionSession _connectionSession;
    private readonly IAuthContext _authContext;

    private bool _authenticated;

    public ClientChannelInboundHandler(IDatabaseProtocolClientEngine databaseProtocolClientEngine,
        ISocketChannel channel,IAppRuntimeManager appRuntimeManager,IMessageCommandProcessor messageCommandProcessor)
    {
        _databaseProtocolClientEngine = databaseProtocolClientEngine;
        _appRuntimeManager = appRuntimeManager;
        _messageCommandProcessor = messageCommandProcessor;
        _connectionSession = new ConnectionSession(TransactionTypeEnum.LOCAL, channel,appRuntimeManager);
        _authContext = databaseProtocolClientEngine.GetAuthContext();
    }

    public override void ChannelActive(IChannelHandlerContext context)
    {
        _messageCommandProcessor.Register(context.Channel.Id);
        var connectionId = _databaseProtocolClientEngine.GetAuthenticationHandler().Handshake(context,_authContext);
        _connectionSession.SetConnectionId(connectionId);
    }


    public override void ChannelRead(IChannelHandlerContext ctx, object msg)
    {
        var byteBuffer = (IByteBuffer)msg;
        if (!_authenticated && !Volatile.Read(ref _authenticated))
        {
            _authenticated = Authenticate(ctx, byteBuffer);
            return;
        }

        var cmd =
            new MessageCommand(_databaseProtocolClientEngine, _connectionSession, ctx, byteBuffer);
        if (!_messageCommandProcessor.TryReceived(ctx.Channel.Id,cmd))
        {
            _logger.LogError($"cant process message command,processor maybe un register: \n{ByteBufferUtil.PrettyHexDump(byteBuffer)}");
        }
    }

    private bool Authenticate(IChannelHandlerContext context, IByteBuffer message)
    {
        using (var payload = _databaseProtocolClientEngine.GetPacketCodec()
                   .CreatePacketPayload(message, context.Channel.GetEncoding()))
        {
            try
            {
                var authenticationResult = _databaseProtocolClientEngine.GetAuthenticationHandler()
                    .Authenticate(context, payload,_authContext);
                if (authenticationResult.Finished)
                {
                    _connectionSession.SetGrantee(new Grantee(authenticationResult.Username!,
                        authenticationResult.Hostname));
                    _connectionSession.SetCurrentDatabaseName(authenticationResult.Database);
                }

                return authenticationResult.Finished;
            }
            catch (Exception e)
            {
                Console.WriteLine("exception occur:");
                Console.WriteLine($"{e}");
                context.WriteAndFlushAsync(new MySqlErrPacket(1, MySqlServerErrorCode.ER_NO_DB_ERROR));
                context.CloseAsync();
            }
        }

        return false;
    }

    public override void ChannelWritabilityChanged(IChannelHandlerContext context)
    {
        _logger.LogWarning(
            $"current channel writable changed:{context.Channel.IsWritable},connection id:{_connectionSession.GetConnectionId()}");
        if (context.Channel.IsWritable)
        {
            _connectionSession.NotifyChannelIsWritable();
        }
    }

    /// <summary>
    /// 当连接关闭后
    /// </summary>
    /// <param name="context"></param>
    public override void ChannelInactive(IChannelHandlerContext context)
    {
        context.FireChannelInactive();
        _connectionSession.Dispose();
        _messageCommandProcessor.UnRegister(context.Channel.Id);
    }
}