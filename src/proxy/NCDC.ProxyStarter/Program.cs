using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;
using NCDC.Basic.TableMetadataManagers;
using NCDC.Host;
using NCDC.Logger;
using NCDC.MySqlParser;
using NCDC.ProxyClient;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClient.Codecs;
using NCDC.ProxyClientMySql;
using NCDC.ProxyClientMySql.ClientConnections;
using NCDC.ProxyClientMySql.Codec;
using NCDC.ProxyServer;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.Metadatas;
using NCDC.ProxyServer.Contexts;
using NCDC.ProxyServer.Executors;
using NCDC.ProxyServer.Options;
using NCDC.ProxyServer.ServerDataReaders;
using NCDC.ProxyServer.ServerHandlers;
using NCDC.ShardingParser;
using NCDC.ShardingRewrite;
using NCDC.ShardingRoute;

namespace NCDC.ProxyStarter
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
   _  __  _____  ___    _____
  / |/ / / ___/ / _ \  / ___/
 /    / / /__  / // / / /__  
/_/|_/  \___/ /____/  \___/  
.Net Core Distributed Connector                            
-------------------------------------------------------------------
Author           :  xuejiaming
Version          :  0.0.1
Github Repository:  https://github.com/xuejmnet/NCDC
Gitee Repository :  https://gitee.com/xuejm/NCDC
-------------------------------------------------------------------
Start Time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

            //注册常用编码
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var port = GetPort(args);
            InternalNCDCLoggerFactory.DefaultFactory = _loggerFactory;
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
            serivces.AddSingleton<IContextManager,DefaultContextManager>();
            serivces.AddSingleton<IServiceHost,DefaultServiceHost>();
            serivces.AddSingleton<IPacketCodec,MySqlPacketCodecEngine>();
            serivces.AddSingleton<IDatabaseProtocolClientEngine,MySqlClientEngine>();
            serivces.AddSingleton<IClientDbConnection,MySqlClientDbConnection>();
            serivces.AddSingleton<IServerHandlerFactory,ServerHandlerFactory>();
            serivces.AddSingleton<IServerDataReaderFactory,ServerDataReaderFactory>();
            serivces.Configure<ShardingProxyOption>(_configuration);
            
         var logicDatabase = new LogicDatabase("a");
         logicDatabase.AddDataSource("ds0", "123", MySqlConnectorFactory.Instance, true);
         var shardingRuntimeContext = new ShardingRuntimeContext("a");
         shardingRuntimeContext.Services.AddSingleton<ILogicDatabase>(logicDatabase);
         shardingRuntimeContext.Services.AddSingleton<ITableMetadataManager,TableMetadataManager>();
         shardingRuntimeContext.Services.AddSingleton<IShardingExecutionContextFactory,ShardingExecutionContextFactory>();
         shardingRuntimeContext.Services.AddShardingParser();
         shardingRuntimeContext.Services.AddMySqlParser();
         shardingRuntimeContext.Services.AddShardingRoute();
         shardingRuntimeContext.Services.AddShardingRewrite();
         serivces.AddSingleton(sp =>
         {
             shardingRuntimeContext.Build();
             return shardingRuntimeContext;
         });
            var buildServiceProvider = serivces.BuildServiceProvider();
            var shardingProxyOption = buildServiceProvider.GetRequiredService<ShardingProxyOption>();
            
            await StartAsync(buildServiceProvider,shardingProxyOption, GetPort(args));
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