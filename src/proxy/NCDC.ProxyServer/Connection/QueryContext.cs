using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DDL;
using NCDC.CommandParser.Common.Command.DML;
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
    public bool DirectAllDataSourceSql { get; }

    public QueryContext(IConnectionSession connectionSession,ISqlCommandContext<ISqlCommand> sqlCommandContext,string sql,ParameterContext parameterContext)
    {
        ConnectionSession = connectionSession;
        SqlCommandContext = sqlCommandContext;
        Sql = sql;
        ParameterContext = parameterContext;
        DirectAllDataSourceSql=IsDirectAllDataSourceSql(sqlCommandContext);
    }
    
    private bool IsDirectAllDataSourceSql(ISqlCommandContext<ISqlCommand> sqlCommandContext)
    {
        var sqlCommand = sqlCommandContext.GetSqlCommand();
        if (sqlCommand is IDMLCommand || sqlCommand is IDDLCommand)
        {
            return false;
        }

        return true;
    }
}