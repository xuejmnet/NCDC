using Microsoft.Extensions.DependencyInjection;
using NCDC.ShardingParser.Abstractions;

namespace NCDC.ShardingParser;

public static class NCDCShardingParserExtension
{
    public static IServiceCollection AddShardingParser(this IServiceCollection services)
    {
        services.AddSingleton<ISqlCommandContextFactory, SqlCommandContextFactory>();
        return services;
    }
}