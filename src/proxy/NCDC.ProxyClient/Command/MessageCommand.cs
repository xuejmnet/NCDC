using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using NCDC.Logger;
using NCDC.Protocol.MySql.Constant;
using NCDC.Protocol.MySql.Packet.Generic;
using NCDC.Protocol.Packets;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyServer;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.Extensions;

namespace NCDC.ProxyClient.Command;

public sealed class MessageCommand:ICommand
{
    private static readonly ILogger<MessageCommand> _logger = NCDCLoggerFactory.CreateLogger<MessageCommand>();
    private readonly IDatabaseProtocolClientEngine _databaseProtocolClientEngine;
    private readonly IConnectionSession _connectionSession;
    private readonly IChannelHandlerContext _context;
    private readonly IByteBuffer _message;
    private readonly bool _logDebug;

    public MessageCommand(IDatabaseProtocolClientEngine databaseProtocolClientEngine,
        IConnectionSession connectionSession, IChannelHandlerContext context, IByteBuffer message)
    {
        _databaseProtocolClientEngine = databaseProtocolClientEngine;
        _connectionSession = connectionSession;
        _context = context;
        _message = message;
        _logDebug = _logger.IsEnabled(LogLevel.Debug);
    }

    public  async ValueTask ExecuteAsync()
    {
        bool isNeedFlush = false;
        bool sqlShow = true;
        using (var payload = _databaseProtocolClientEngine.GetPacketCodec().CreatePacketPayload(_message,
                   _context.Channel.GetEncoding()))
        {
            try
            {
                isNeedFlush =  await ExecuteCommandAsync(_context, payload);
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
    }

    private void ProcessException(Exception exception)
    {
        // if (!ExpectedExceptions.isExpected(cause.getClass())) {
        //     log.error("Exception occur: ", cause);
        // }
        _logger.LogError($"Exception occur: {exception}");
        var clientDbConnection = _databaseProtocolClientEngine.GetClientDbConnection();
         _context.WriteAsync(clientDbConnection.GetErrorPacket(exception));
        var databasePacket = clientDbConnection.GetOtherPacket(_connectionSession);
        if (null != databasePacket)
        {
             _context.WriteAsync(databasePacket);
        }

        _context.Flush();
    }

    private async ValueTask<bool> ExecuteCommandAsync(IChannelHandlerContext context, IPacketPayload payload)
    {
        var clientDbConnection = _databaseProtocolClientEngine.GetClientDbConnection();
        using (var clientCommand =
               clientDbConnection.CreateCommand(payload, _connectionSession))
        {
            try
            {
                using (var clientDataReader = clientCommand.ExecuteReader())
                {
                    
                    var responsePackets = clientDataReader.SendCommand();

                    int i = 0;
                   await foreach (var responsePacket in responsePackets)
                    {
                       _= context.WriteAsync(responsePacket);
                       i++;
                    }

                    if (i == 0)
                    {
                        return false;
                    }

                    if (clientDataReader is IClientQueryDataReader clientQueryDataReader)
                    {
                      await  clientDbConnection.WriteQueryDataAsync(context, _connectionSession,clientQueryDataReader, i);
                    }
                    return true;
                }
                
            }
            catch (Exception e)
            {
                _logger.LogError($"client command execute error:{e}");
                _databaseProtocolClientEngine.HandleException(_connectionSession, e);
                throw;
            }
        }
    }
}