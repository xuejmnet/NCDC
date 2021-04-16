using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using ShardingConnector.Contexts;
using ShardingConnector.Exceptions;
using ShardingConnector.Executor;
using ShardingConnector.Parser.Binder.Command.DML;
using ShardingConnector.Parser.Sql.Command.DAL.Dialect;
using ShardingConnector.ShardingAdoNet.AdoNet.Core.DataReader;
using ShardingConnector.ShardingAdoNet.Executor;

namespace ShardingConnector.ShardingAdoNet
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 21 March 2021 11:04:39
* @Email: 326308290@qq.com
*/
    public class ShardingCommand : DbCommand
    {
        private readonly CommandExecutor _commandExecutor;

        private bool returnGeneratedKeys;

        private ExecutionContext executionContext;

        private DbDataReader currentResultSet;

        public ShardingCommand(string commandText, ShardingConnection connection)
        {
            this.CommandText = commandText;
            this.DbConnection = connection;
            _commandExecutor = new CommandExecutor();
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        public override object ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        public override void Prepare()
        {
            throw new NotImplementedException();
        }

        public override string CommandText { get; set; }
        public override int CommandTimeout { get; set; }
        public override CommandType CommandType { get; set; }
        public override UpdateRowSource UpdatedRowSource { get; set; }
        protected override DbConnection DbConnection { get; set; }
        protected override DbParameterCollection DbParameterCollection { get; }
        protected override DbTransaction DbTransaction { get; set; }
        public override bool DesignTimeVisible { get; set; }

        protected override DbParameter CreateDbParameter()
        {
            throw new NotImplementedException();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            if (string.IsNullOrWhiteSpace(this.CommandText))
                throw new ShardingException("sql command text null or empty");

            if (null != currentResultSet)
            {
                return currentResultSet;
            }

            if (executionContext.GetSqlStatementContext() is SelectCommandContext selectCommandContext || executionContext.GetSqlStatementContext().GetSqlCommand() is DALCommand)
            {
                List<DbDataReader> resultSets = GetDataReaders();
                MergedResult mergedResult = mergeQuery(getQueryResults(resultSets));
                currentResultSet = new ShardingDataReader(resultSets, mergedResult, this, executionContext);
            }

            return currentResultSet;
        }

        private List<DbDataReader> GetDataReaders()
        {
            // List<ResultSet> result = new ArrayList<>(statementExecutor.getStatements().size());
            List<DbDataReader> result = new List<DbDataReader>();
            //     for (Statement each : statementExecutor.getStatements()) {
            //     result.add(each.getResultSet());
            // }
            //     foreach (var VARIABLE in COLLECTION)
            //     {
            //         
            //     }
            return result;
        }
        
        private List<IQueryEnumerator> GetQueryEnumerators(List<DbDataReader> dbDataReaders)
        {
            List<IQueryEnumerator> result = new List<IQueryEnumerator>(dbDataReaders.Count);
        for (ResultSet each : resultSets) {
            if (null != each) {
                result.add(new StreamQueryResult(each));
            }
        }
        foreach (var dataReader in dbDataReaders)
        {
            if (null != dataReader)
            {
                result.Add();
            }
        }
    return result;
}
    }
}