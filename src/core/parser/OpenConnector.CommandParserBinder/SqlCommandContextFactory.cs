using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Command;
using OpenConnector.CommandParser.Command.DML;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.Command.DML;
using OpenConnector.CommandParserBinder.MetaData.Schema;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.CommandParserBinder
{
/*
* @Author: xjm
* @Description:
* @Date: Thursday, 08 April 2021 21:51:28
* @Email: 326308290@qq.com
*/
    public static class SqlCommandContextFactory
    {
        public static ISqlCommandContext<ISqlCommand> Create(SchemaMetaData schemaMetaData, string sql, ParameterContext parameterContext, ISqlCommand sqlCommand) {
            if(sqlCommand is DMLCommand dmlCommand)
            {
                return GetDMLCommandContext(schemaMetaData, sql, parameterContext, dmlCommand);
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

        private static ISqlCommandContext<ISqlCommand> GetDMLCommandContext(SchemaMetaData schemaMetaData, string sql, ParameterContext parameterContext, DMLCommand sqlCommand)
        {
            if (sqlCommand is SelectCommand selectCommand)
            {
                return new SelectCommandContext(schemaMetaData, sql, parameterContext, selectCommand);
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
                return new InsertCommandContext(schemaMetaData, parameterContext, insertCommand);
            }
            throw new NotSupportedException($"Unsupported SQL statement `{sqlCommand.GetType().Name}`");
        }
    }
}