using Microsoft.Extensions.DependencyInjection;
using NCDC.ProxyClient;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClient.Authentication;
using NCDC.ProxyClient.Codecs;
using NCDC.ProxyClientMySql;
using NCDC.ProxyClientMySql.Authentication;
using NCDC.ProxyClientMySql.ClientConnections;
using NCDC.ProxyClientMySql.Codec;

namespace Microsoft.Extensions.DependencyInjection;

public static class DIExtension
{
    public static IServiceCollection AddProxyClientMySql(this IServiceCollection services)
    {
        services.AddProxyClientCore();
        services.AddSingleton<IPacketCodec, MySqlPacketCodecEngine>();
        services.AddSingleton<IDatabaseProtocolClientEngine, MySqlClientEngine>();
        services.AddSingleton<IClientDbConnection, MySqlClientDbConnection>();
        services.AddSingleton<IAuthenticationHandler, MySqlAuthenticationHandler>();
        return services;
    }
    
}