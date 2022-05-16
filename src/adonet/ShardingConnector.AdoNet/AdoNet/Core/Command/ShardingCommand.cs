using ShardingConnector.AdoNet.AdoNet.Core.Connection;
using ShardingConnector.AdoNet.Executor;
using ShardingConnector.Exceptions;
using ShardingConnector.Executor;
using ShardingConnector.Merge.Reader;
using ShardingConnector.Pluggable.Merge;
using ShardingConnector.Pluggable.Prepare;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExecutionContext = ShardingConnector.Executor.Context.ExecutionContext;
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
            _commandExecutor = new CommandExecutor(connection);
        }
        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override int ExecuteNonQuery()
        {
            try
            {
                executionContext = Prepare(CommandText);
                return _commandExecutor.ExecuteNonQuery();
            }
            finally
            {
                currentResultSet = null;
            }
        }

        public override  Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
        {
            try
            {
                executionContext = Prepare(CommandText);
                var executeNonQuery = _commandExecutor.ExecuteNonQuery();
                return Task.FromResult(executeNonQuery);
            }
            finally
            {
                currentResultSet = null;
            }
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
        protected override DbParameterCollection DbParameterCollection
        {
            get
            {
                if (null == _parameters)
                {
                    // delay the creation of the SqlParameterCollection
                    // until user actually uses the Parameters property
                    _parameters = new ShardingParameterCollection();
                }
                return _parameters;
            }
        }
        protected override DbTransaction DbTransaction { get; set; }
        public override bool DesignTimeVisible { get; set; }

        protected override DbParameter CreateDbParameter()
        {
            return new ShardingParameter();
        }
        public new DbParameter CreateParameter() => this.CreateDbParameter();
        private ShardingParameterCollection _parameters;

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            if (string.IsNullOrWhiteSpace(this.CommandText))
                throw new ShardingException("sql command text null or empty");

            DbDataReader result;
            try
            {
                executionContext = Prepare(CommandText);
                List<IStreamDataReader> queryResults = _commandExecutor.ExecuteQuery();
                IStreamDataReader mergedResult = MergeQuery(queryResults);
                result = new ShardingDataReader(_commandExecutor.DbDataReaders, mergedResult, this, executionContext);
            }
            finally
            {
                currentResultSet = null;
            }
            currentResultSet = result;
            return result;
        }

        private IStreamDataReader MergeQuery(List<IStreamDataReader> streamDataReaders)
        {
            ShardingRuntimeContext runtimeContext = ((ShardingConnection)DbConnection).GetRuntimeContext();
            MergeEngine mergeEngine = new MergeEngine(runtimeContext.GetRule().ToRules(), runtimeContext.GetProperties(), runtimeContext.GetDatabaseType(), runtimeContext.GetMetaData().Schema);
            return mergeEngine.Merge(streamDataReaders, executionContext.GetSqlCommandContext());
        }

        private ExecutionContext Prepare(string sql)
        {
            _commandExecutor.Clear();
            ShardingRuntimeContext runtimeContext = ((ShardingConnection)DbConnection).GetRuntimeContext();
            BasePrepareEngine prepareEngine = new SimpleQueryPrepareEngine(
                runtimeContext.GetRule().ToRules(), runtimeContext.GetProperties(), runtimeContext.GetMetaData(), runtimeContext.GetSqlParserEngine());
            ExecutionContext result = prepareEngine.Prepare(sql, _parameters.GetParams().Select(o=>(object)o).ToList());
            _commandExecutor.Init(result);
            //_commandExecutor.Commands.for
            // statementExecutor.getStatements().forEach(this::replayMethodsInvocation);
            return result;
        }
    }
}
