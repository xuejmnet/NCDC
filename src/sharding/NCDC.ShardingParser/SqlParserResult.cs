using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DDL;
using NCDC.CommandParser.Common.Command.DML;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.MetaData;

namespace NCDC.ShardingParser;

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