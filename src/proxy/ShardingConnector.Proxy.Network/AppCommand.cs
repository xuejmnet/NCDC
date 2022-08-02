using System.Text;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using ShardingConnector.Protocol.Core;
using ShardingConnector.Proxy.Network.Servers;

namespace ShardingConnector.Proxy.Network;

public class AppCommand:IAppCommand
{
    private readonly ServerConnection _serverConnection;
    private readonly IChannelHandlerContext _context;
    private readonly object _message;
    private readonly Encoding _encoding;

    public AppCommand(ServerConnection serverConnection,IChannelHandlerContext context,object message,Encoding encoding)
    {
        _serverConnection = serverConnection;
        _context = context;
        _message = message;
        _encoding = encoding;
    }
    public ValueTask ExecuteAsync()
    {
        var connectionSize = 0;
        var isNeedFlush = false;
        using (var serviceConnection = this._serverConnection)
            using(var mySqlPacketPayload=new MySqlPacketPayload((IByteBuffer)_message,_encoding))
        {
            
        }
    }
}