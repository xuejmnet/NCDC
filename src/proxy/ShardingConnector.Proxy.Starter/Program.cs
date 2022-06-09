using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShardingConnector.Logger;
using ShardingConnector.Merge.Engine;
using ShardingConnector.RewriteEngine.Context;
using ShardingConnector.Route;

namespace ShardingConnector.Proxy.Starter
{
    class Program
    {
        private static IConfiguration _configuration =
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        private static ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddSimpleConsole(c => c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss]");
        });

        private const int DEFAULT_PORT = 3307;

        static void Main(string[] args)
        {
            RegisterDecorator();
            InternalLoggerFactory.DefaultFactory = _loggerFactory;
            var serivces = new ServiceCollection();
            serivces.AddSingleton<IConfiguration>(serviceProvider => _configuration);
            serivces.AddSingleton<ILoggerFactory>(serviceProvider => _loggerFactory);

            Console.WriteLine("Hello World!");
        }

        private static void RegisterDecorator()
        {
            NewInstanceServiceLoader.Register<IRouteDecorator>();
            NewInstanceServiceLoader.Register<ISqlRewriteContextDecorator>();
            NewInstanceServiceLoader.Register<IResultProcessEngine>();
        }

        private static int GetPort(string[] args)
        {
            if (args.Length == 0)
            {
                return DEFAULT_PORT;
            }

            return int.TryParse(args[0], out var port) ? port : DEFAULT_PORT;
        }

        private static void Start(IDictionary<string, string> options, int port)
        {
            
        }
    }
}