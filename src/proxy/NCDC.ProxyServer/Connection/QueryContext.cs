using NCDC.CommandParser.Common.Command;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser.Command;

namespace NCDC.ProxyServer.Connection;

public sealed class QueryContext:IQueryContext
{
    public ISqlCommandContext<ISqlCommand> SqlCommandContext { get; }
    public string Sql { get; }
    public ParameterContext ParameterContext { get; }
    public IConnectionSession ConnectionSession { get; }

    public QueryContext(IConnectionSession connectionSession,ISqlCommandContext<ISqlCommand> sqlCommandContext,string sql,ParameterContext parameterContext)
    {
        ConnectionSession = connectionSession;
        SqlCommandContext = sqlCommandContext;
        Sql = sql;
        ParameterContext = parameterContext;
    }
}