


using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NCDC.Host;
using NCDC.Logger;
using NCDC.MySqlParser;
using NCDC.ProxyServer.AppServices;
using NCDC.ProxyServer.AppServices.Abstractions;
using NCDC.ProxyServer.Bootstrappers;
using NCDC.ShardingParser;
using NCDC.WebBootstrapper;
using NCDC.WebBootstrapper.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
builder.Configuration.AddJsonFile("Configs/JwtConfig.json");
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

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    //忽略系统自带校验你[ApiController] 
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpGlobalExceptionFilter>();
    options.Filters.Add<ValidateModelStateFilter>();
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
});
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddSecurity(builder.Configuration);

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

//添加跨域
builder.Services.AddCors(options =>
{
    options.AddPolicy("App4Cors",
        b => b.SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
builder.Services.AddHostedService<AppStarter>();
var app = builder.Build();

// app.UseHttpsRedirection();

app.UseCors("App4Cors");
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

