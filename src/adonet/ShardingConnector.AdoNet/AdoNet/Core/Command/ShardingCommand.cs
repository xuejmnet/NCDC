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
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ShardingConnector.ShardingAdoNet;
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
        private readonly DbConnection _defaultDbConnection;
        private readonly DbCommand _defaultDbCommand;

        public ShardingCommand(string commandText, ShardingConnection connection)
        {
            this.CommandText = commandText;
            this.DbConnection = connection;
            _defaultDbConnection = connection.GetDefaultDbConnection();
            _defaultDbCommand = _defaultDbConnection.CreateCommand();
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
        /// <summary>
        /// 当前命令的链接
        /// </summary>
        protected override DbConnection DbConnection { get; set; }
        protected override DbParameterCollection DbParameterCollection
        {
            get
            {
                if (null == _parameters)
                {
                    // delay the creation of the SqlParameterCollection
                    // until user actually uses the ParameterContext property
                    _parameters = new ShardingParameterCollection();
                }
                return _parameters;
            }
        }
        protected override DbTransaction DbTransaction { get; set; }
        public override bool DesignTimeVisible { get; set; }

        protected override DbParameter CreateDbParameter()
        {
            var dbParameter = _defaultDbCommand.CreateParameter();
            return new ShardingParameter(dbParameter);
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
                List<IStreamDataReader> dataReaders = _commandExecutor.ExecuteQuery();
                IStreamDataReader mergedResult = MergeQuery(dataReaders);
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
            BasePrepareEngine prepareEngine = HasAnyParams() ? (BasePrepareEngine)new PreparedQueryPrepareEngine(
                runtimeContext.GetRule().ToRules(), runtimeContext.GetProperties(), runtimeContext.GetMetaData(), runtimeContext.GetSqlParserEngine())
                    : (BasePrepareEngine)new SimpleQueryPrepareEngine(
                        runtimeContext.GetRule().ToRules(), runtimeContext.GetProperties(), runtimeContext.GetMetaData(), runtimeContext.GetSqlParserEngine());
            //Stopwatch sp=Stopwatch.StartNew();
            //for (int i = 0; i < 1000; i++)
            //{
            //    BasePrepareEngine prepareEngine1 = new PreparedQueryPrepareEngine(
            //        runtimeContext.GetRule().ToRules(), runtimeContext.GetProperties(), runtimeContext.GetMetaData(), runtimeContext.GetSqlParserEngine());
            //    ExecutionContext result1 = prepareEngine1.Prepare(sql, _parameters.GetParams().Select(o => (object)o).ToList());
            //}
            //sp.Stop();
            //Console.WriteLine(sp.ElapsedMilliseconds);
            var parameterContext = new ParameterContext(_parameters?.GetParams().Select(o=>(DbParameter)o).ToArray()??Array.Empty<DbParameter>());
            ExecutionContext result = prepareEngine.Prepare(sql, parameterContext);
            _commandExecutor.Init(result);
            //_commandExecutor.Commands.for
            // statementExecutor.getStatements().forEach(this::replayMethodsInvocation);
            return result;
        }

        private bool HasAnyParams()
        {
            return _parameters != null && _parameters.GetParams().Any();
        }
    }
}
