using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DML;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser.Abstractions;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.Command.DML;

namespace NCDC.ShardingParser
{
/*
* @Author: xjm
* @Description:
* @Date: Thursday, 08 April 2021 21:51:28
* @Email: 326308290@qq.com
*/
    public  class SqlCommandContextFactory:ISqlCommandContextFactory
    {
        public  ISqlCommandContext<ISqlCommand> Create(ParameterContext parameterContext, ISqlCommand sqlCommand) {
            if(sqlCommand is IDMLCommand dmlCommand)
            {
                return GetDMLCommandContext(parameterContext, dmlCommand);
            }
           
            //if (sqlCommand is DMLStatement) {
            //    return getDMLStatementContext(schemaMetaData, sql, parameters, (DMLStatement) sqlStatement);
            //}
            //if (sqlStatement instanceof DDLStatement) {
            //    return getDDLStatementContext((DDLStatement) sqlStatement);
            //}
            //if (sqlStatement instanceof DCLStatement) {
            //    return getDCLStatementContext((DCLStatement) sqlStatement);
            //}
            //if (sqlStatement instanceof DALStatement) {
            //    return getDALStatementContext((DALStatement) sqlStatement);
            //}
            return new GenericSqlCommandContext<ISqlCommand>(sqlCommand);
        }

        private  ISqlCommandContext<ISqlCommand> GetDMLCommandContext(ParameterContext parameterContext, IDMLCommand sqlCommand)
        {
            if (sqlCommand is SelectCommand selectCommand)
            {
                return new SelectCommandContext(parameterContext, selectCommand);
            }
            if (sqlCommand is UpdateCommand updateCommand)
            {
                return new UpdateCommandContext(updateCommand);
            }
            if (sqlCommand is DeleteCommand deleteCommand)
            {
                return new DeleteCommandContext(deleteCommand);
            }
            if (sqlCommand is InsertCommand insertCommand) {
                return new InsertCommandContext(parameterContext, insertCommand);
            }
            throw new NotSupportedException($"Unsupported SQL statement `{sqlCommand.GetType().Name}`");
        }
    }
}