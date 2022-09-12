using Microsoft.Extensions.DependencyInjection;
using NCDC.CommandParser;
using NCDC.CommandParser.Abstractions;
using NCDC.MySqlParser;

namespace NCDC.MySqlParser;

public static class NCDCMySqlParserExtension
{
    public static IServiceCollection AddMySqlParser(this IServiceCollection services)
    {
        services.AddCommandParser();
        services.AddSingleton<ISqlParserConfiguration, MySqlParserConfiguration>();
        return services;
    }
}