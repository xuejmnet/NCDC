using System.Text;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Logging;
using OpenConnector.Configuration.Session;
using OpenConnector.Configuration.User;
using OpenConnector.Logger;
using OpenConnector.Protocol.MySql.Constant;
using OpenConnector.Protocol.MySql.Packet.Generic;
using OpenConnector.ProxyClient.Command;
using OpenConnector.ProxyServer;
using OpenConnector.ProxyServer.Abstractions;
using OpenConnector.ProxyServer.Session;
using OpenConnector.Transaction;

namespace OpenConnector.ProxyClient.DotNetty;

public class ClientChannelInboundHandler : ChannelHandlerAdapter
{
    private static readonly ILogger<ClientChannelInboundHandler> _logger =
        InternalLoggerFactory.CreateLogger<ClientChannelInboundHandler>();

    private readonly IDatabaseProtocolClientEngine _databaseProtocolClientEngine;
    private readonly ICommandListener _commandListener;

    private readonly ConnectionSession _connectionSession;
    // private readonly IAuthenticationEngine _authenticationEngine = new MySqlAuthenticationEngine();

    private bool _authenticated;

    public ClientChannelInboundHandler(IDatabaseProtocolClientEngine databaseProtocolClientEngine,
        ISocketChannel channel, ICommandListener commandListener)
    {
        _databaseProtocolClientEngine = databaseProtocolClientEngine;
        _commandListener = commandListener;
        _connectionSession = new ConnectionSession(TransactionTypeEnum.LOCAL, channel);
    }

    public override void ChannelActive(IChannelHandlerContext context)
    {
        var connectionId = _databaseProtocolClientEngine.GetAuthenticationEngine().Handshake(context);
        Console.WriteLine($"connectionId:{connectionId}");
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