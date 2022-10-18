using NCDC.CommandParser.Common.Command;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser.Command;

namespace NCDC.ProxyServer.Connection;

public sealed class QueryContext
{
    public ISqlCommandContext<ISqlCommand> SqlCommandContext { get; }
    public string Sql { get; }
    public ParameterContext ParameterContext { get; }

    public QueryContext(ISqlCommandContext<ISqlCommand> sqlCommandContext,string sql,ParameterContext parameterContext)
    {
        SqlCommandContext = sqlCommandContext;
        Sql = sql;
        ParameterContext = parameterContext;
    }
}