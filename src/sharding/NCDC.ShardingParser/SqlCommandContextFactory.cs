using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command.DML;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.Command.DML;
using NCDC.Basic.TableMetadataManagers;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser.Abstractions;

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
        private readonly ITableMetadataManager _tableMetadataManager;

        public SqlCommandContextFactory(ITableMetadataManager tableMetadataManager)
        {
            _tableMetadataManager = tableMetadataManager;
        }
        public  ISqlCommandContext<ISqlCommand> Create(string sql, ParameterContext parameterContext, ISqlCommand sqlCommand) {
            if(sqlCommand is DMLCommand dmlCommand)
            {
                return GetDMLCommandContext(sql, parameterContext, dmlCommand);
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

        private  ISqlCommandContext<ISqlCommand> GetDMLCommandContext(string sql, ParameterContext parameterContext, DMLCommand sqlCommand)
        {
            if (sqlCommand is SelectCommand selectCommand)
            {
                return new SelectCommandContext(_tableMetadataManager, sql, parameterContext, selectCommand);
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
                return new InsertCommandContext(_tableMetadataManager, parameterContext, insertCommand);
            }
            throw new NotSupportedException($"Unsupported SQL statement `{sqlCommand.GetType().Name}`");
        }
    }
}