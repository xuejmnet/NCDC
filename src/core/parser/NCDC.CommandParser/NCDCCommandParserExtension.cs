using Microsoft.Extensions.DependencyInjection;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Core;
using NCDC.CommandParser.SqlParseEngines;

namespace NCDC.CommandParser;

public static class NCDCCommandParserExtension
{
    public static IServiceCollection AddCommandParser(this IServiceCollection services)
    {
        services.AddSingleton<ISqlCommandParser, SqlCommandParser>();
        return services;
    }
}