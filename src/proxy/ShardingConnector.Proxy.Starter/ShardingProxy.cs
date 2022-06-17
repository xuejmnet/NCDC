using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Libuv;
using Microsoft.Extensions.Logging;
using ShardingConnector.Logger;
using ShardingConnector.Proxy.Common;
using ShardingConnector.Proxy.Network;
using LogLevel = DotNetty.Handlers.Logging.LogLevel;

namespace ShardingConnector.Proxy.Starter;

public class ShardingProxy:IShardingProxy
{
    private static readonly ILogger<ShardingProxy> _logger = InternalLoggerFactory.CreateLogger<ShardingProxy>();
    private readonly ShardingProxyOption _shardingProxyOption;
    private readonly ServerHandlerInitializer _serverHandlerInitializer;

    // 主工作线程组，设置为1个线程
    private IEventLoopGroup bossGroup;

    // 工作线程组，默认为内核数*2的线程数
    private IEventLoopGroup workerGroup;

    /// <summary>
    /// 服务启动
    /// </summary>
    private ServerBootstrap bootstrap;

    public ShardingProxy(ShardingProxyOption shardingProxyOption,ServerHandlerInitializer serverHandlerInitializer)
    {
        _shardingProxyOption = shardingProxyOption;
        _serverHandlerInitializer = serverHandlerInitializer;
    }
    public async Task StartAsync(CancellationToken cancellationToken = default)
    { 
        _logger.LogInformation("----------开始启动----------");
        _logger.LogInformation($"----------监听端口:{_shardingProxyOption.Port}----------");
        var dispatcher = new DispatcherEventLoopGroup();
        bossGroup = dispatcher;
        workerGroup = new WorkerEventLoopGroup(dispatcher);
        try
        {
            
                //声明一个服务端Bootstrap，每个Netty服务端程序，都由ServerBootstrap控制，
                //通过链式的方式组装需要的参数
                bootstrap = new ServerBootstrap();
                bootstrap
                    .Group(bossGroup, workerGroup) // 设置主和工作线程组
                    .Channel<TcpServerChannel>() // 设置通道模式为TcpSocket
                    .Option(ChannelOption.TcpNodelay, true)
                    .Option(ChannelOption.SoReuseaddr, true)
                    .Option(ChannelOption.SoBacklog, 100) // 看最下面解释
                    .Option(ChannelOption.SoKeepalive, true) //保持连接
                    .Option(ChannelOption.WriteBufferLowWaterMark, 8 * 1024 * 1024)
                    .Option(ChannelOption.WriteBufferHighWaterMark, 16 * 1024 * 1024)
                    // .Option(ChannelOption.ConnectTimeout, TimeSpan.FromSeconds(3)) //连接超时
                    .Option(ChannelOption.RcvbufAllocator, new AdaptiveRecvByteBufAllocator(1024, 1024, 65536))
                    .Handler(new LoggingHandler(LogLevel.INFO))
                    .ChildHandler(_serverHandlerInitializer);
                    // .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                    // {
                    //     //工作线程连接器 是设置了一个管道，服务端主线程所有接收到的信息都会通过这个管道一层层往下传输
                    //     //同时所有出栈的消息 也要这个管道的所有处理器进行一步步处理
                    //     IChannelPipeline pipeline = channel.Pipeline;
                    //     // pipeline.AddLast("tls", TlsHandler.Server(_option.TlsCertificate));
                    //     // pipeline.AddLast("tls", new TlsHandler(stream => new SslStream(stream, true, (sender, certificate, chain, errors) => true), new ClientTlsSettings(_targetHost)));
                    //     // pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(int.MaxValue, 0, 4, 0, 4));
                    //     // pipeline.AddLast(new MessagePackDecoder());
                    //     // pipeline.AddLast("framing-enc", new LengthFieldPrepender(4, false));
                    //     //实体类编码器,心跳管理器,连接管理器
                    //     // pipeline.AddLast(new MessagePackEncoder()
                    //     //     , new IdleStateHandler(0, 0, _option.AllIdleTime),
                    //     //     new NettyServerConnectManagerHandler(), new NettyServerHandler(_serviceProvider));
                    // }));

                // bootstrap绑定到指定端口的行为 就是服务端启动服务，同样的Serverbootstrap可以bind到多个端口
                await bootstrap.BindAsync(_shardingProxyOption.Port);
                _logger.LogInformation($"----------启动完成端口:{_shardingProxyOption.Port}----------");
            }
            catch (Exception ex)
            {
                _logger.LogError($"----------启动异常:{ex}----------");
            }

    }

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