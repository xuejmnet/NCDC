using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenConnector.Host;
using OpenConnector.Logger;
using OpenConnector.Merge.Engine;
using OpenConnector.ProxyClient;
using OpenConnector.ProxyClient.Abstractions;
using OpenConnector.ProxyClient.Codecs;
using OpenConnector.ProxyClient.Command;
using OpenConnector.ProxyClientMySql;
using OpenConnector.ProxyClientMySql.ClientConnections;
using OpenConnector.ProxyClientMySql.Codec;
using OpenConnector.ProxyServer;
using OpenConnector.ProxyServer.Abstractions;
using OpenConnector.ProxyServer.Commons;
using OpenConnector.ProxyServer.Options;
using OpenConnector.ProxyServer.ServerDataReaders;
using OpenConnector.ProxyServer.ServerHandlers;
using OpenConnector.RewriteEngine.Context;
using OpenConnector.Route;
using OpenConnector.ShardingCommon.Core.Rule;

namespace OpenConnector.Proxy.Starter
{
    class Program
    {
        private static IConfiguration _configuration =
            new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        private static ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddSimpleConsole(c => c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss]")
                .AddFilter(level=>level>=LogLevel.Debug);
        });

        private const int DEFAULT_PORT = 3307;

        static async Task Main(string[] args)
        {
            Console.WriteLine(@$"
   ____                      ______                            __            
  / __ \____  ___  ____     / ____/___  ____  ____  ___  _____/ /_____  _____
 / / / / __ \/ _ \/ __ \   / /   / __ \/ __ \/ __ \/ _ \/ ___/ __/ __ \/ ___/
/ /_/ / /_/ /  __/ / / /  / /___/ /_/ / / / / / / /  __/ /__/ /_/ /_/ / /    
\____/ .___/\___/_/ /_/   \____/\____/_/ /_/_/ /_/\___/\___/\__/\____/_/     
    /_/                                                                      
-------------------------------------------------------------------
Author           :  xuejiaming
Version          :  0.0.1
Github Repository:  https://github.com/xuejmnet/NCDC
Gitee Repository :  https://gitee.com/xuejm/NCDC
-------------------------------------------------------------------
Start Time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

            //注册常用编码
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ProxyContext.Init();
            RegisterDecorator();
            var port = GetPort(args);
            InternalLoggerFactory.DefaultFactory = _loggerFactory;
            var serivces = new ServiceCollection();
            serivces.AddSingleton<IConfiguration>(serviceProvider => _configuration);
            serivces.AddSingleton<ILoggerFactory>(serviceProvider => _loggerFactory);
            serivces.AddSingleton<ShardingProxyOption>(serviceProvider =>
            {
                var proxyOption = serviceProvider.GetRequiredService<IOptionsSnapshot<ShardingProxyOption>>().Value;
                if (port.HasValue)
                {
                    proxyOption.Port = port.Value;
                }

                return proxyOption;
            });
            // serivces.AddSingleton<ServerHandlerInitializer>();
            // serivces.AddSingleton<PackDecoder>();
            // serivces.AddSingleton<PackEncoder>();
            // serivces.AddSingleton<ApplicationChannelInboundHandler>();
            // serivces.AddSingleton<IShardingProxy,ShardingProxy>();
            serivces.AddSingleton<IServiceHost,DefaultServiceHost>();
            serivces.AddSingleton<IPacketCodec,MySqlPacketCodecEngine>();
            serivces.AddSingleton<IDatabaseProtocolClientEngine,MySqlClientEngine>();
            serivces.AddSingleton<IClientDbConnection,MySqlClientDbConnection>();
            // serivces.AddSingleton<IClientDataReaderFactory,MySqlClientDataReaderFactory>();
            // serivces.AddSingleton<IServerConnector,AdoNetSer>();
            serivces.AddSingleton<IServerHandlerFactory,ServerHandlerFactory>();
            serivces.AddSingleton<IServerDataReaderFactory,ServerDataReaderFactory>();
            serivces.Configure<ShardingProxyOption>(_configuration);
            var buildServiceProvider = serivces.BuildServiceProvider();
            var shardingProxyOption = buildServiceProvider.GetRequiredService<ShardingProxyOption>();
            var authentication = new Authentication();
            authentication.Users.Add("root",new ProxyUser("123456",new List<string>(){"test"}));
            ShardingProxyContext.GetInstance().Init(authentication,new Dictionary<string, string>());
            
            ProxyRuntimeContext.Instance.Init(BuildRuntimeOption());
            await StartAsync(buildServiceProvider,shardingProxyOption, GetPort(args));
        }

        private static void RegisterDecorator()
        {
            NewInstanceServiceLoader.Register<IRouteDecorator>();
            NewInstanceServiceLoader.Register<ISqlRewriteContextDecorator>();
            NewInstanceServiceLoader.Register<IResultProcessEngine>();
        }

        private static int? GetPort(string[] args)
        {
            if (args.Length == 0)
            {
                return null;
            }

            return int.TryParse(args[0], out var port) ? port : null;
        }

        private static async Task StartAsync(IServiceProvider serviceProvider,ShardingProxyOption option, int? port)
        {
            var host = serviceProvider.GetRequiredService<IServiceHost>();
           await host.StartAsync();
           while (Console.ReadLine()!="quit")
           {
               Console.WriteLine("unknown input params");
           }

           await host.StopAsync();
           Console.WriteLine("open connector safe quit");
        }

        static ProxyRuntimeOption BuildRuntimeOption()
        {
            var proxyRuntimeOption = new ProxyRuntimeOption();
            var userOption = new UserOption();
            userOption.Username = "xjm";
            userOption.Password = "abc";
            userOption.Databases.Add("xxa");
            proxyRuntimeOption.Users.Add(userOption);
            var databaseOption = new DatabaseOption();
            databaseOption.Name = "xxa";
            var dataSourceOption = new DataSourceOption();
            dataSourceOption.DataSourceName = "ds0";
            dataSourceOption.ConnectionString = "server=127.0.0.1;port=3306;database=test;userid=root;password=root;";
            dataSourceOption.IsDefault = true;
            databaseOption.DataSources.Add(dataSourceOption);
            proxyRuntimeOption.Databases.Add(databaseOption);
            return proxyRuntimeOption;
        }
    }
}