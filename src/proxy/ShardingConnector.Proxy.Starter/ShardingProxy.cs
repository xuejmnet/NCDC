using System.Net;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Libuv;
using Microsoft.Extensions.Logging;
using ShardingConnector.Logger;
using ShardingConnector.ProtocolCore.Codecs;
using ShardingConnector.ProtocolCore.DotNetty;
using ShardingConnector.ProxyClient;
using ShardingConnector.ProxyClient.DotNetty;
using ShardingConnector.ProxyServer;
using LogLevel = DotNetty.Handlers.Logging.LogLevel;

namespace ShardingConnector.Proxy.Starter;

public class ShardingProxy:IShardingProxy
{
    private static readonly ILogger<ShardingProxy> _logger = InternalLoggerFactory.CreateLogger<ShardingProxy>();
 
    private readonly ShardingProxyOption _shardingProxyOption;
    private readonly IDatabasePacketCodecEngine _databasePacketCodecEngine;
    private readonly IDatabaseProtocolClientEngine _databaseProtocolClientEngine;

    // 主工作线程组，设置为1个线程
    private IEventLoopGroup bossGroup;

    // 工作线程组，默认为内核数*2的线程数
    private IEventLoopGroup workerGroup;

    /// <summary>
    /// 服务启动
    /// </summary>
    private ServerBootstrap _serverBootstrap;
    private Bootstrap _clientBootstrap;

    public ShardingProxy(ShardingProxyOption shardingProxyOption,IDatabaseProtocolClientEngine databaseProtocolClientEngine)
    {
        _shardingProxyOption = shardingProxyOption;
        _databaseProtocolClientEngine = databaseProtocolClientEngine;
        _databasePacketCodecEngine = databaseProtocolClientEngine.GetCodecEngine();
    }
    public async Task StartAsync(CancellationToken cancellationToken = default)
    { 
        _logger.LogInformation("----------开始启动----------");
        _logger.LogInformation($"----------监听端口:{_shardingProxyOption.Port}----------");
        // var dispatcher = new DispatcherEventLoopGroup();
        // bossGroup = dispatcher;
        // workerGroup = new WorkerEventLoopGroup(dispatcher);

        _clientBootstrap = new Bootstrap();
        bossGroup =new MultithreadEventLoopGroup(1);
        workerGroup = new MultithreadEventLoopGroup();
        try
        {
            _clientBootstrap.Group(new MultithreadEventLoopGroup());
            _clientBootstrap
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Option(ChannelOption.SoBacklog, 100) // 看最下面解释
                .Option(ChannelOption.SoKeepalive, true) //保持连接
                .Option(ChannelOption.ConnectTimeout, TimeSpan.FromSeconds(3)) //连接超时
                .Option(ChannelOption.RcvbufAllocator, new AdaptiveRecvByteBufAllocator(1024, 1024, 65536));
                //声明一个服务端Bootstrap，每个Netty服务端程序，都由ServerBootstrap控制，
                //通过链式的方式组装需要的参数
                _serverBootstrap = new ServerBootstrap();
                _serverBootstrap
                    .Group(bossGroup, workerGroup) // 设置主和工作线程组
                    .Channel<TcpServerSocketChannel>() // 设置通道模式为TcpSocket
                    .Option(ChannelOption.TcpNodelay, true)
                    .Option(ChannelOption.SoReuseaddr, true)
                    .Option(ChannelOption.SoBacklog, 100) // 看最下面解释
                    .Option(ChannelOption.SoKeepalive, true) //保持连接
                    .Option(ChannelOption.WriteBufferLowWaterMark, 8 * 1024 * 1024)
                    .Option(ChannelOption.WriteBufferHighWaterMark, 16 * 1024 * 1024)
                    .Option(ChannelOption.ConnectTimeout, TimeSpan.FromSeconds(3)) //连接超时
                    .Option(ChannelOption.RcvbufAllocator, new AdaptiveRecvByteBufAllocator(1024, 1024, 65536))
                    .Handler(new LoggingHandler(LogLevel.INFO))
                    // .ChildHandler(_serverHandlerInitializer);
                    .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        //工作线程连接器 是设置了一个管道，服务端主线程所有接收到的信息都会通过这个管道一层层往下传输
                        //同时所有出栈的消息 也要这个管道的所有处理器进行一步步处理
                        IChannelPipeline pipeline = channel.Pipeline;
                        pipeline.AddLast(new ChannelAttrInitializer());
                        pipeline.AddLast(new MessagePacketDecoder(_databasePacketCodecEngine));
                        pipeline.AddLast(new MessagePacketEncoder(_databasePacketCodecEngine));
                        pipeline.AddLast(new ConnectorManagerHandler());
                        pipeline.AddLast(new ClientChannelInboundHandler(_databaseProtocolClientEngine,channel));
                        // pipeline.AddLast("tls", TlsHandler.Server(_option.TlsCertificate));
                        // pipeline.AddLast("tls", new TlsHandler(stream => new SslStream(stream, true, (sender, certificate, chain, errors) => true), new ClientTlsSettings(_targetHost)));
                        // pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(int.MaxValue, 0, 4, 0, 4));
                        // pipeline.AddLast(new MessagePacketDecoder());
                        // pipeline.AddLast("framing-enc", new LengthFieldPrepender(4, false));
                        //实体类编码器,心跳管理器,连接管理器
                        // pipeline.AddLast(new MessagePacketEncoder()
                        //     , new IdleStateHandler(0, 0, _option.AllIdleTime),
                        //     new NettyServerConnectManagerHandler(), new NettyServerHandler(_serviceProvider));
                    }));

                // bootstrap绑定到指定端口的行为 就是服务端启动服务，同样的Serverbootstrap可以bind到多个端口
                await _serverBootstrap.BindAsync(_shardingProxyOption.Port);
                _logger.LogInformation($"----------启动完成端口:{_shardingProxyOption.Port}----------");
            }
            catch (Exception ex)
            {
                _logger.LogError($"----------启动异常:{ex}----------");
            }

    }

    // private IChannel GetClientChannel(IChannel tcpSocketChannel)
    // {
    //
    //     try
    //     {
    //         var ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3306);
    //         var result = this._clientBootstrap.Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
    //         {
    //             var channelPipeline = channel.Pipeline;
    //             channelPipeline.AddLast(new ApplicationChannelInboundHandler());
    //         })).ConnectAsync(ipEndPoint).GetAwaiter().GetResult();
    //         return result;
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         throw;
    //     }
    // }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("----------开始停止----------");
            await bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            await workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            _logger.LogInformation("----------已停止----------");
        }
        catch (Exception e)
        {
            _logger.LogInformation($"----------停止异常:{e}----------");
        }
    }
}