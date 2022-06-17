using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace ShardingConnector.Proxy.Network;

public sealed class ServerHandlerInitializer:ChannelInitializer<TcpSocketChannel>
{
    private readonly PackDecoder _packDecoder;
    private readonly PackEncoder _packEncoder;

    public ServerHandlerInitializer(PackDecoder packDecoder,PackEncoder packEncoder)
    {
        _packDecoder = packDecoder;
        _packEncoder = packEncoder;
    }
    protected override void InitChannel(TcpSocketChannel channel)
    {
        var pipeline = channel.Pipeline;
        pipeline.AddLast(_packDecoder);
        pipeline.AddLast(_packDecoder);
    }
}