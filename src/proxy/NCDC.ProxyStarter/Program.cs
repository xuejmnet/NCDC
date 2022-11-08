using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NCDC.Host;
using NCDC.Logger;
using NCDC.MySqlParser;
using NCDC.ProxyServer.AppServices.Abstractions;
using NCDC.ProxyServer.Bootstrappers;
using NCDC.ShardingParser;

namespace NCDC.ProxyStarter
{
    class Program
    {
        private static IConfiguration _configuration =
            new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json")
                .Build();

        private static ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddSimpleConsole(c => c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss]")
                .AddFilter(level => level >= LogLevel.Debug);
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
            NCDCLoggerFactory.DefaultFactory = _loggerFactory;
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(serviceProvider => _configuration);
            services.AddSingleton<ILoggerFactory>(serviceProvider => _loggerFactory);

            services.AddSingleton<IServiceHost, DefaultServiceHost>();
            services.AddEntityFrameworkCoreConfiguration();
            services.AddProxyClientMySql();
            services.AddMySqlParser();
            services.AddShardingParser();
            
            var buildServiceProvider = services.BuildServiceProvider();
            var argPort = GetPort(args);
            if (argPort.HasValue)
            {
                var appConfiguration = buildServiceProvider.GetRequiredService<IAppConfiguration>();
                appConfiguration.Port = argPort.Value;
            }
            await StartAsync(buildServiceProvider);
        }

        private static int? GetPort(string[] args)
        {
            if (args.Length == 0)
            {
                return null;
            }

            return int.TryParse(args[0], out var port) ? port : null;
        }

        private static async Task StartAsync(IServiceProvider serviceProvider)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var appBootstrapper = serviceProvider.GetRequiredService<IAppBootstrapper>();
            cancellationTokenSource.Token.Register(() =>
            {
                appBootstrapper.StopAsync(cancellationTokenSource.Token).Wait(TimeSpan.FromSeconds(30000));
            });
            await appBootstrapper.StartAsync(cancellationTokenSource.Token);
       
            while (!cancellationTokenSource.IsCancellationRequested&&Console.ReadLine() != "quit")
            {
                Console.WriteLine("unknown input params");
            }
            cancellationTokenSource.Cancel();
            Console.WriteLine("ncdc quit");
        }
    }
}