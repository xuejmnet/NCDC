using System;

namespace ShardingConnector.Kernels.Parse.SqlExpression
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
        public static ISqlCommandContext<ISqlCommand> NewInstance(final SchemaMetaData schemaMetaData, final String sql, final List<Object> parameters, final SQLStatement sqlStatement) {
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
    }
}