


using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NCDC.Host;
using NCDC.Logger;
using NCDC.MySqlParser;
using NCDC.ProxyServer.AppServices;
using NCDC.ProxyServer.AppServices.Abstractions;
using NCDC.ShardingParser;
using NCDC.WebBootstrapper;

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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ILoggerFactory loggerFactory = LoggerFactory.Create(b =>
// {
//     b
//         .AddSimpleConsole(c => c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss]");
// });

//注册常用编码
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
// NCDCLoggerFactory.DefaultFactory = loggerFactory;
builder.Logging.AddSimpleConsole(c => c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss]");
// builder.Services.Replace(ServiceDescriptor.Singleton<ILoggerFactory>(sp => loggerFactory));
builder.Services.AddControllers();

builder.Services.AddSingleton<IServiceHost, DefaultServiceHost>();
builder.Services.AddEntityFrameworkCoreConfiguration();
builder.Services.AddProxyClientMySql();
builder.Services.AddMySqlParser();
builder.Services.AddShardingParser();
builder.Services.AddSingleton<IAppConfiguration>(sp =>
{

    var configuration = builder.Configuration;
    var database = configuration["DatabaseType"];
    var storage = configuration["StorageType"];
    var connectionString = configuration["ConnectionString"];
    var port = configuration["Port"];
    var routePluginPath = configuration["RoutePluginPath"];
    var logEncode = configuration["LogEncode"];
    var logDecode = configuration["LogDecode"];
    return new AppConfiguration()
    {
        DatabaseType = database.ParseDatabaseType(),
        StorageType = storage.ParseStorageType(),
        ConnectionsString = connectionString,
        Port = port.ParseInt(),
        RulePluginPath = routePluginPath,
        LogEncode = logEncode.ParseBool(),
        LogDecode = logDecode.ParseBool()
    };
});
builder.Services.AddHostedService<AppStarter>();
var app = builder.Build();
NCDCLoggerFactory.DefaultFactory = app.Services.GetRequiredService<ILoggerFactory>();  

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

