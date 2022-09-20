using System.Threading.Channels;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Logging;
using NCDC.Host;
using NCDC.Logger;
using NCDC.ProxyClient;
using NCDC.ProxyClient.Codecs;
using NCDC.ProxyClient.Command;
using NCDC.ProxyClient.DotNetty;
using NCDC.ProxyServer;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Contexts;

namespace NCDC.ProxyStarter;

public class DefaultServiceHost:IServiceHost
{
    private static readonly ILogger<DefaultServiceHost> _logger = InternalNCDCLoggerFactory.CreateLogger<DefaultServiceHost>();
 
    // private readonly ICommandListener _commandListener= new DefaultCommandListener();
    private readonly IChannelHandler _connectorManagerHandler= new ConnectorManagerHandler();
    private readonly ShardingProxyOption _shardingProxyOption;
    private readonly IPacketCodec _packetCodec;
    private readonly IDatabaseProtocolClientEngine _databaseProtocolClientEngine;
    private readonly IContextManager _contextManager;
    // private readonly Channel<MessageCommand> _messageChannel;

    // 主工作线程组，设置为1个线程
    private IEventLoopGroup bossGroup;

    // 工作线程组，默认为内核数*2的线程数
    private IEventLoopGroup workerGroup;

    /// <summary>
    /// 服务启动
    /// </summary>
    private ServerBootstrap _serverBootstrap;
    private Bootstrap _clientBootstrap;
    private IChannelHandler _encoderHandler;

    public DefaultServiceHost(ShardingProxyOption shardingProxyOption,IDatabaseProtocolClientEngine databaseProtocolClientEngine,IContextManager contextManager)
    {
        _shardingProxyOption = shardingProxyOption;
        _databaseProtocolClientEngine = databaseProtocolClientEngine;
        _contextManager = contextManager;
        _packetCodec = databaseProtocolClientEngine.GetPacketCodec();
        _encoderHandler = new MessagePacketEncoder(_packetCodec);
        // _commandListener.OnReceived += CommandListenerOnReceived;
        // _messageChannel = Channel.CreateUnbounded<MessageCommand>();
    }

    public async Task StartAsync()
    {
        _logger.LogInformation("----------开始启动----------");
        _logger.LogInformation($"----------监听端口:{_shardingProxyOption.Port}----------");


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
                    .Option(ChannelOption.WriteBufferLowWaterMark, 8 * 1024 * 1024)//8mb用来控制流量flush
                    .Option(ChannelOption.WriteBufferHighWaterMark, 16 * 1024 * 1024)//16mb用来控制流量flush
                    .Option(ChannelOption.ConnectTimeout, TimeSpan.FromSeconds(3)) //连接超时
                    .Option(ChannelOption.RcvbufAllocator, new AdaptiveRecvByteBufAllocator(1024, 1024, 65536))
                    // .Handler(new LoggingHandler(LogLevel.INFO))
                    // .ChildHandler(_serverHandlerInitializer);
                    .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        //工作线程连接器 是设置了一个管道，服务端主线程所有接收到的信息都会通过这个管道一层层往下传输
                        //同时所有出栈的消息 也要这个管道的所有处理器进行一步步处理
                        IChannelPipeline pipeline = channel.Pipeline;
                        pipeline.AddLast(new ChannelAttrInitializer());
                        pipeline.AddLast(new MessagePacketDecoder(_packetCodec));
                        pipeline.AddLast(_encoderHandler);
                        pipeline.AddLast(_connectorManagerHandler);
                        pipeline.AddLast(new ClientChannelInboundHandler(_databaseProtocolClientEngine,channel,_contextManager));
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
                await _serverBootstrap.BindAsync(_shardingProxyOption.Port).ConfigureAwait(false);
                _logger.LogInformation($"----------启动完成端口:{_shardingProxyOption.Port}----------");
            }
            catch (Exception ex)
            {
                _logger.LogError($"----------启动异常:{ex}----------");
            }

    }

    public async Task StopAsync()
    {
        try
        {
            _logger.LogInformation("----------开始停止----------");
            await bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)).ConfigureAwait(false);
            await workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)).ConfigureAwait(false);
            _logger.LogInformation("----------已停止----------");
        }
        catch (Exception e)
        {
            _logger.LogInformation($"----------停止异常:{e}----------");
        }
    }
    private ValueTask CommandListenerOnReceived(ICommand command)
    {
        return command.ExecuteAsync();
    }
}