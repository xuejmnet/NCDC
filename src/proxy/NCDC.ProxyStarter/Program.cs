using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;
using NCDC.Basic.Configurations;
using NCDC.Basic.Metadatas;
using NCDC.Basic.TableMetadataManagers;
using NCDC.Enums;
using NCDC.Host;
using NCDC.Logger;
using NCDC.MySqlParser;
using NCDC.ProxyClient;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClient.Authentication;
using NCDC.ProxyClient.Codecs;
using NCDC.ProxyClient.Command;
using NCDC.ProxyClient.Command.Abstractions;
using NCDC.ProxyClientMySql;
using NCDC.ProxyClientMySql.Authentication;
using NCDC.ProxyClientMySql.ClientConnections;
using NCDC.ProxyClientMySql.Codec;
using NCDC.ProxyServer;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.Metadatas;
using NCDC.ProxyServer.Connection.User;
using NCDC.ProxyServer.Contexts;
using NCDC.ProxyServer.Executors;
using NCDC.ProxyServer.Options;
using NCDC.ProxyServer.ServerDataReaders;
using NCDC.ProxyServer.ServerHandlers;
using NCDC.ShardingMerge;
using NCDC.ShardingMerge.Abstractions;
using NCDC.ShardingParser;
using NCDC.ShardingRewrite;
using NCDC.ShardingRoute;
using NCDC.ShardingRoute.TableRoutes.Abstractions;
using DatabaseOption = NCDC.ProxyServer.Options.DatabaseOption;
using DataSourceOption = NCDC.ProxyServer.Options.DataSourceOption;
using UserOption = NCDC.ProxyServer.Options.UserOption;

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
            serivces.AddSingleton<IMessageCommandProcessor, TaskMessageCommandProcessor>();
            serivces.AddSingleton<IMessageExecutorFactory, MessageExecutorFactory>();
            serivces.AddSingleton<IContextManager, DefaultContextManager>();
            serivces.AddSingleton<IServiceHost, DefaultServiceHost>();
            serivces.AddSingleton<IPacketCodec, MySqlPacketCodecEngine>();
            serivces.AddSingleton<IDatabaseProtocolClientEngine, MySqlClientEngine>();
            serivces.AddSingleton<IClientDbConnection, MySqlClientDbConnection>();
            serivces.AddSingleton<IServerHandlerFactory, ServerHandlerFactory>();
            serivces.AddSingleton<IServerDataReaderFactory, ServerDataReaderFactory>();
            serivces.AddSingleton<IAuthenticationHandler, MySqlAuthenticationHandler>();
            serivces.AddSingleton<IUserManager, DefaultUserManager>();
            serivces.Configure<ShardingProxyOption>(_configuration);
            var shardingConfiguration = new ShardingConfiguration();
            shardingConfiguration.AddDefaultDataSource("ds0",
                "server=127.0.0.1;port=3306;database=test;userid=root;password=root;");

            var logicDatabase = new LogicDatabase("xxa");
            logicDatabase.AddDataSource("ds0", "server=127.0.0.1;port=3306;database=test;userid=root;password=root;",
                MySqlConnectorFactory.Instance, true);

            var shardingRuntimeContext = new ShardingRuntimeContext("xxa");
            shardingRuntimeContext.Services.AddSingleton<ILogicDatabase>(logicDatabase);
            shardingRuntimeContext.Services.AddSingleton<ITableMetadataManager, TableMetadataManager>();
            shardingRuntimeContext.Services.AddSingleton<IDataReaderMergerFactory, DataReaderMergerFactory>();
            shardingRuntimeContext.Services.AddSingleton<IDatabaseSettings>(sp =>
                new DatabaseSettings("xxa", DatabaseTypeEnum.MySql));
            shardingRuntimeContext.Services
                .AddSingleton<IShardingExecutionContextFactory, ShardingExecutionContextFactory>();
            shardingRuntimeContext.Services.AddShardingParser();
            shardingRuntimeContext.Services.AddMySqlParser();
            shardingRuntimeContext.Services.AddShardingRoute();
            shardingRuntimeContext.Services.AddShardingRewrite();
            shardingRuntimeContext.Services.AddSingleton(sp => shardingConfiguration);
            serivces.AddSingleton<IRuntimeContext>(sp =>
            {
                shardingRuntimeContext.Build();
                return shardingRuntimeContext;
            });
            var buildServiceProvider = serivces.BuildServiceProvider();
            var userManager = buildServiceProvider.GetRequiredService<IUserManager>();
            userManager.AddUser(new AuthUser("xjm", "abc", "%"));
            var shardingProxyOption = buildServiceProvider.GetRequiredService<ShardingProxyOption>();
            await StartAsync(buildServiceProvider, shardingProxyOption, GetPort(args));
        }

        private static int? GetPort(string[] args)
        {
            if (args.Length == 0)
            {
                return null;
            }

            return int.TryParse(args[0], out var port) ? port : null;
        }

        private static async Task StartAsync(IServiceProvider serviceProvider, ShardingProxyOption option, int? port)
        {
            var contextManager = serviceProvider.GetRequiredService<IContextManager>();
            var databaseNames = contextManager.GetAllDatabaseNames();
            foreach (var databaseName in databaseNames)
            {
                var runtimeContext = contextManager.GetRuntimeContext(databaseName);
                var tableMetadataManager = runtimeContext.GetTableMetadataManager();
                var tableMetadata = new TableMetadata("sysusermod", new Dictionary<string, ColumnMetadata>()
                {
                    { "id", new ColumnMetadata("id", 0, "varchar", true, false, true) },
                    { "name", new ColumnMetadata("name", 1, "varchar", false, false, true) },
                    { "age", new ColumnMetadata("age", 2, "int", false, false, true) },
                });
                tableMetadata.SetShardingTableColumn("id");
                tableMetadata.AddActualTableWithDataSource("ds0", "sysusermod_00");
                tableMetadata.AddActualTableWithDataSource("ds0", "sysusermod_01");
                tableMetadata.AddActualTableWithDataSource("ds0", "sysusermod_02");
                tableMetadataManager.AddTableMetadata(tableMetadata);
                var tableRouteManager = runtimeContext.GetTableRouteManager();
                var testModTableRoute = runtimeContext.CreateInstance<TestModTableRoute>();
                tableRouteManager.AddRoute(testModTableRoute);
            }

            var host = serviceProvider.GetRequiredService<IServiceHost>();
            await host.StartAsync();
            while (Console.ReadLine() != "quit")
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

    public class TestModTableRoute : AbstractOperatorTableRoute
    {
        public TestModTableRoute(ITableMetadataManager tableMetadataManager) : base(tableMetadataManager)
        {
        }

        public override string TableName => "sysusermod";

        public override Func<string, bool> GetRouteToFilter(IComparable shardingValue,
            ShardingOperatorEnum shardingOperator)
        {
            var tail = FormatTableName(shardingValue);
            var table = $"{TableName}{GetTableMetadata().TableSeparator}{tail}";

            switch (shardingOperator)
            {
                case ShardingOperatorEnum.EQUAL: return t => t.EndsWith(table);
                default:
                {
                    return t => true;
                }
            }
        }

        public string FormatTableName(IComparable shardingValue)
        {
            var shardingKey = $"{shardingValue}";
            var stringHashCode = GetStringHashCode(shardingKey) % 3;
            return stringHashCode.ToString().PadLeft(2, '0');
        }

        public static int GetStringHashCode(string value)
        {
            Check.NotNull(value, nameof(value));
            int h = 0; // 默认值是0
            if (value.Length > 0)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    h = 31 * h + value[i]; // val[0]*31^(n-1) + val[1]*31^(n-2) + ... + val[n-1]
                }
            }

            return h;
        }
    }
}