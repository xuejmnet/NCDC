using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using ShardingConnector.Extensions;
using ShardingConnector.Logger;
using ShardingConnector.ProtocolCore;
using ShardingConnector.ProtocolCore.Packets.Executor;
using ShardingConnector.ProtocolCore.Payloads;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClient.Command;

public sealed class ClientCommand : IClientCommand
{
    private static readonly ILogger<ClientCommand> _logger = InternalLoggerFactory.CreateLogger<ClientCommand>();
    private readonly IDatabaseProtocolClientEngine _databaseProtocolClientEngine;
    private readonly ConnectionSession _connectionSession;
    private readonly IChannelHandlerContext _context;
    private readonly IByteBuffer _message;
    private readonly bool _logDebug;

    public ClientCommand(IDatabaseProtocolClientEngine databaseProtocolClientEngine,
        ConnectionSession connectionSession, IChannelHandlerContext context, IByteBuffer message)
    {
        _databaseProtocolClientEngine = databaseProtocolClientEngine;
        _connectionSession = connectionSession;
        _context = context;
        _message = message;
        _logDebug = _logger.IsEnabled(LogLevel.Debug);
    }

    public  ValueTask ExecuteAsync()
    {
        bool isNeedFlush = false;
        bool sqlShow = true;
        using (var payload = _databaseProtocolClientEngine.GetCodecEngine().CreatePacketPayload(_message,
                   _context.Channel.GetAttribute(CommonConstants.CHARSET_ATTRIBUTE_KEY).Get()))
        {
            try
            {
                isNeedFlush =  ExecuteCommand(_context, payload);
            }
            catch (Exception exception)
            {
                 ProcessException(exception);
            }
            finally
            {
                if (isNeedFlush)
                {
                    _context.Flush();
                }
            }
        }
        return ValueTask.CompletedTask;
    }

    private void ProcessException(Exception exception)
    {
        // if (!ExpectedExceptions.isExpected(cause.getClass())) {
        //     log.error("Exception occur: ", cause);
        // }
        _logger.LogError($"Exception occur: {exception}");
        var commandExecuteEngine = _databaseProtocolClientEngine.GetCommandExecuteEngine();
         _context.WriteAsync(commandExecuteEngine.GetErrorPacket(exception));
        var databasePacket = commandExecuteEngine.GetOtherPacket(_connectionSession);
        if (null != databasePacket)
        {
             _context.WriteAsync(databasePacket);
        }

        _context.Flush();
    }

    private bool ExecuteCommand(IChannelHandlerContext context, IPacketPayload payload)
    {
        var commandExecuteEngine = _databaseProtocolClientEngine.GetCommandExecuteEngine();
        using (var commandExecutor =
               commandExecuteEngine.GetCommandExecutor(payload, _connectionSession))
        {
            try
            {
                var responsePackets = commandExecutor.Execute();
                if (responsePackets.IsEmpty())
                {
                    return false;
                }

                foreach (var responsePacket in responsePackets)
                {
                     context.WriteAsync(responsePacket);
                }

                // if (commandExecutor is IQueryCommandExecutor queryCommandExecutor)
                // {
                //     commandExecuteEngine.WriteQueryData(context, _connectionSession.ServerConnection,
                //         queryCommandExecutor, responsePackets.Count);
                // }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _databaseProtocolClientEngine.HandleException(_connectionSession, e);
                throw;
            }
        }
    }
}