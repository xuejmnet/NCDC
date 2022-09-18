using System.Text;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Logging;
using NCDC.Enums;
using NCDC.Logger;
using NCDC.Protocol.MySql.Constant;
using NCDC.Protocol.MySql.Packet.Generic;
using NCDC.ProxyClient.Authentication;
using NCDC.ProxyClient.Command;
using NCDC.ProxyServer;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.Connection.User;
using NCDC.ProxyServer.Contexts;

namespace NCDC.ProxyClient.DotNetty;

public class ClientChannelInboundHandler : ChannelHandlerAdapter
{
    private static readonly ILogger<ClientChannelInboundHandler> _logger =
        InternalNCDCLoggerFactory.CreateLogger<ClientChannelInboundHandler>();

    private readonly IDatabaseProtocolClientEngine _databaseProtocolClientEngine;
    private readonly ICommandListener _commandListener;
    private readonly IContextManager _contextManager;
    private readonly IAuthenticator _authenticator;

    private readonly IConnectionSession _connectionSession;

    private bool _authenticated;

    public ClientChannelInboundHandler(IDatabaseProtocolClientEngine databaseProtocolClientEngine,
        ISocketChannel channel, ICommandListener commandListener,IContextManager contextManager,IAuthenticatorFactory authenticatorFactory)
    {
        _databaseProtocolClientEngine = databaseProtocolClientEngine;
        _commandListener = commandListener;
        _contextManager = contextManager;
        _authenticator = authenticatorFactory.Create();
        _connectionSession = new ConnectionSession(TransactionTypeEnum.LOCAL, channel,contextManager);
    }

    public override void ChannelActive(IChannelHandlerContext context)
    {
        var connectionId = _databaseProtocolClientEngine.GetAuthenticationEngine().Handshake(context);
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

        var commandMessageSender =
            new MessageCommand(_databaseProtocolClientEngine, _connectionSession, ctx, byteBuffer);
        Task.Run(async () => await _commandListener.Received(commandMessageSender).ConfigureAwait(false));
    }

    private bool Authenticate(IChannelHandlerContext context, IByteBuffer message)
    {
        using (var payload = _databaseProtocolClientEngine.GetPacketCodec()
                   .CreatePacketPayload(message, context.GetAttribute(CommonConstants.CHARSET_ATTRIBUTE_KEY).Get()))
        {
            try
            {
                var authenticationResult = _databaseProtocolClientEngine.GetAuthenticationEngine()
                    .Authenticate(context, payload);
                if (authenticationResult.Finished)
                {
                    _connectionSession.SetGrantee(new Grantee(authenticationResult.Username,
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
}