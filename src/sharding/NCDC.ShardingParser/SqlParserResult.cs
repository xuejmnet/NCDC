using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
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
    public bool NoRouteReWriteSql { get; }
    public bool DefaultDataSourceExecute { get; }

    public SqlParserResult(string sql,ISqlCommandContext<ISqlCommand> sqlCommandContext,ParameterContext parameterContext,ITableMetadataManager tableMetadataManager)
    {
        Sql = sql;
        SqlCommandContext = sqlCommandContext;
        ParameterContext = parameterContext;
        NoRouteReWriteSql = IsNoRouteReWriteSql(sqlCommandContext);
        if (!NoRouteReWriteSql)
        {
            DefaultDataSourceExecute = IsDefaultDataSourceExecute(sqlCommandContext, tableMetadataManager);
        }
    }

    /// <summary>
    /// 原生sql无需重写无需路由直接执行
    /// </summary>
    /// <param name="sqlCommandContext"></param>
    /// <returns></returns>
    private bool IsNoRouteReWriteSql(ISqlCommandContext<ISqlCommand> sqlCommandContext)
    {
        if (sqlCommandContext.GetSqlCommand() is IDMLCommand)
        {
            return false;
        }

        return true;
    }
    private bool IsDefaultDataSourceExecute(ISqlCommandContext<ISqlCommand> sqlCommandContext,ITableMetadataManager tableMetadataManager)
    {
        if (sqlCommandContext.GetSqlCommand() is IDMLCommand)
        {
            var tableNames = sqlCommandContext.GetTablesContext().GetTableNames();
            return tableNames.Any(tableMetadataManager.IsSharding);
        }

        return false;
    }
}