// using DotNetty.Buffers;
// using DotNetty.Transport.Channels;
// using DotNetty.Transport.Channels.Sockets;
//
// namespace ShardingConnector.Proxy.Network;
//
// public sealed class ServerHandlerInitializer:ChannelInitializer<TcpSocketChannel>
// {
//     private readonly PackDecoder _packDecoder;
//     private readonly PackEncoder _packEncoder;
//     // private readonly ApplicationChannelInboundHandler _applicationChannelInboundHandler;
//
//     public override bool IsSharable => true;
//
//     public ServerHandlerInitializer(PackDecoder packDecoder,PackEncoder packEncoder)
//     {
//         _packDecoder = packDecoder;
//         _packEncoder = packEncoder;
//         // _applicationChannelInboundHandler = applicationChannelInboundHandler;
//     }
//     protected override void InitChannel(TcpSocketChannel channel)
//     {
//         var pipeline = channel.Pipeline;
//         pipeline.AddLast(_packDecoder);
//         pipeline.AddLast(_packEncoder);
//         pipeline.AddLast();
//     }
//
//     private IChannel GetChannel(TcpSocketChannel channel)
//     {
//         (IByteBuffer)ms
//     }
// }