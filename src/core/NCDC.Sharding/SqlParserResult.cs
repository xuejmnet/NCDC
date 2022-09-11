using NCDC.CommandParser.Abstractions;
using NCDC.ShardingParser.Command;
using NCDC.ShardingAdoNet;

namespace NCDC.Sharding;

public sealed class SqlParserResult
{
    public string Sql { get; }
    public ISqlCommandContext<ISqlCommand> SqlCommandContext { get; }
    public ParameterContext ParameterContext { get; }

    public SqlParserResult(string sql,ISqlCommandContext<ISqlCommand> sqlCommandContext,ParameterContext parameterContext)
    {
        Sql = sql;
        SqlCommandContext = sqlCommandContext;
        ParameterContext = parameterContext;
    }
}