using ShardingConnector.AdoNet.AdoNet.Core.Connection;
using ShardingConnector.AdoNet.Executor;
using ShardingConnector.Exceptions;
using ShardingConnector.Executor;
using ShardingConnector.Executor.Context;
using ShardingConnector.Merge.Reader;
using ShardingConnector.Pluggable.Merge;
using ShardingConnector.Pluggable.Prepare;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using ShardingDataReader = ShardingConnector.AdoNet.AdoNet.Core.DataReader.ShardingDataReader;
using ShardingRuntimeContext = ShardingConnector.AdoNet.AdoNet.Core.Context.ShardingRuntimeContext;

namespace ShardingConnector.AdoNet.AdoNet.Core.Command
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/19 13:57:02
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class ShardingCommand: DbCommand
    {
        private readonly CommandExecutor _commandExecutor;

        private bool returnGeneratedKeys;

        private ExecutionContext executionContext;

        private DbDataReader currentResultSet;

        public ShardingCommand(string commandText, ShardingConnection connection)
        {
            this.CommandText = commandText;
            this.DbConnection = connection;
            _commandExecutor = new CommandExecutor(connection);
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

            DbDataReader result;
            try
            {
                executionContext = Prepare(CommandText);
                List<IQueryEnumerator> queryResults = _commandExecutor.ExecuteQuery();
                IMergedEnumerator mergedResult = MergeQuery(queryResults);
                result = new ShardingDataReader(_commandExecutor.ResultSets, mergedResult, this, executionContext);
            }
            finally
            {
                currentResultSet = null;
            }
            currentResultSet = result;
            return result;
        }

        private IMergedEnumerator MergeQuery(List<IQueryEnumerator> queryResults)
        {
            ShardingRuntimeContext runtimeContext = ((ShardingConnection)DbConnection).GetRuntimeContext();
            MergeEngine mergeEngine = new MergeEngine(runtimeContext.GetRule().ToRules(), runtimeContext.GetProperties(), runtimeContext.GetDatabaseType(), runtimeContext.GetMetaData().Schema);
            return mergeEngine.Merge(queryResults, executionContext.GetSqlStatementContext());
        }

        private ExecutionContext Prepare(string sql)
        {
            _commandExecutor.Clear();
            ShardingRuntimeContext runtimeContext = ((ShardingConnection)DbConnection).GetRuntimeContext();
            BasePrepareEngine prepareEngine = new SimpleQueryPrepareEngine(
                runtimeContext.GetRule().ToRules(), runtimeContext.GetProperties(), runtimeContext.GetMetaData(), runtimeContext.GetSqlParserEngine());
            ExecutionContext result = prepareEngine.Prepare(sql, new List<object>());
            _commandExecutor.Init(result);
            // statementExecutor.getStatements().forEach(this::replayMethodsInvocation);
            return result;
        }
    }
}
