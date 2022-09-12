using Microsoft.Extensions.DependencyInjection;
using NCDC.ShardingRewrite.Abstractions;
using NCDC.ShardingRewrite.ParameterRewriters;

namespace NCDC.ShardingRewrite;

public static class NCDCShardingRewriteExtension
{
    public static IServiceCollection AddShardingRewrite(this IServiceCollection services)
    {
        services.AddSingleton<IParameterRewriterBuilder, ShardingParameterRewriterBuilder>();
        services.AddSingleton<ISqlRewriterContextFactory, SqlRewriterContextFactory>();
        return services;
    }
}