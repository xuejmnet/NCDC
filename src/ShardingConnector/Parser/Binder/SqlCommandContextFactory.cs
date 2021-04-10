using System.Collections.Generic;
using ShardingConnector.Kernels.MetaData.Schema;
using ShardingConnector.Kernels.Parse.SqlExpression;
using ShardingConnector.Parser.Binder.Command;
using ShardingConnector.Parser.Sql.Command;
using ShardingConnector.Parser.Sql.Command.DML;

namespace ShardingConnector.Parser.Binder
{
/*
* @Author: xjm
* @Description:
* @Date: Thursday, 08 April 2021 21:51:28
* @Email: 326308290@qq.com
*/
    public class SqlCommandContextFactory
    {
        private SqlCommandContextFactory(){}
        public static ISqlCommandContext<ISqlCommand> NewInstance(SchemaMetaData schemaMetaData, string sql, List<object> parameters, ISqlCommand sqlCommand) {
            if(sqlCommand is DMLCommand dmlCommand)
            {
                return GetDMLStatementContext(schemaMetaData, sql, parameters, dmlCommand);
            }
            if (sqlStatement instanceof DMLStatement) {
                return getDMLStatementContext(schemaMetaData, sql, parameters, (DMLStatement) sqlStatement);
            }
            if (sqlStatement instanceof DDLStatement) {
                return getDDLStatementContext((DDLStatement) sqlStatement);
            }
            if (sqlStatement instanceof DCLStatement) {
                return getDCLStatementContext((DCLStatement) sqlStatement);
            }
            if (sqlStatement instanceof DALStatement) {
                return getDALStatementContext((DALStatement) sqlStatement);
            }
            return new CommonSQLStatementContext(sqlStatement);
        }

        private static ISqlCommandContext<ISqlCommand> GetDMLStatementContext(SchemaMetaData schemaMetaData, string sql, List<object> parameters, DMLCommand sqlCommand)
        {
            if(sqlCommand is SelectCommand selectCommand)
            if (sqlStatement instanceof SelectStatement) {
                return new SelectStatementContext(schemaMetaData, sql, parameters, selectCommand);
            }
            if (sqlStatement instanceof UpdateStatement) {
                return new UpdateStatementContext((UpdateStatement)sqlStatement);
            }
            if (sqlStatement instanceof DeleteStatement) {
                return new DeleteStatementContext((DeleteStatement)sqlStatement);
            }
            if (sqlStatement instanceof InsertStatement) {
                return new InsertStatementContext(schemaMetaData, parameters, (InsertStatement)sqlStatement);
            }
            throw new UnsupportedOperationException(String.format("Unsupported SQL statement `%s`", sqlStatement.getClass().getSimpleName()));
        }
    }
}