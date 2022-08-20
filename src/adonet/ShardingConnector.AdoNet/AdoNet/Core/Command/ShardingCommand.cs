using ShardingConnector.AdoNet.AdoNet.Core.Connection;
using ShardingConnector.AdoNet.Executor;
using ShardingConnector.Exceptions;
using ShardingConnector.Pluggable.Merge;
using ShardingConnector.Pluggable.Prepare;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using ShardingConnector.AdoNet.AdoNet.Abstraction;
using ShardingConnector.AdoNet.Executor.Abstractions;
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
    public class ShardingCommand : DbCommand, IAdoMethodRecorder<DbCommand>
    {

        private bool returnGeneratedKeys;

        private ExecutionContext executionContext;

        private DbDataReader currentResultSet;
        private readonly ICommandExecutor _commandExecutor;
        private ShardingConnection _shardingConnection;

        public ShardingCommand():this(null)
        {
            
        }
        public ShardingCommand(string commandText):this(commandText,null)
        {
        }
        public ShardingCommand(string commandText, ShardingConnection connection)
        {
            _shardingConnection = connection;
            _commandExecutor = CreateCommandExecutor(10);
        }

        private ICommandExecutor CreateCommandExecutor(int maxQueryConnectionsLimit)
        {
            var commandExecutor = new DefaultCommandExecutor(maxQueryConnectionsLimit);
            commandExecutor.OnGetConnections += (c, s, i) =>
            {
                if (_shardingConnection != null)
                {
                    return _shardingConnection.GetConnections(c, s, i);
                }

                throw new ShardingException(
                    $"{nameof(ShardingCommand)} {nameof(_shardingConnection)} is null");
            };
            return commandExecutor;
        }

        public override void Cancel()
        {
            RecordTargetMethodInvoke(command=>command.Cancel());
        }

        public override int ExecuteNonQuery()
        {
            try
            {
                executionContext = Prepare(CommandText);
                return _commandExecutor.ExecuteNonQuery(true,executionContext).Sum();
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
            RecordTargetMethodInvoke(command=>command.Prepare());
        }

        public override string CommandText { get; set; }

        private int _commandTimeout;
        public override int CommandTimeout
        {
            get => _commandTimeout;
            set
            {
                RecordTargetMethodInvoke(command => command.CommandTimeout = value);
                _commandTimeout = value;
            }
        }

        private CommandType _commandType=CommandType.Text;
        public override CommandType CommandType
        {
            get => _commandType;
            set
            {
                RecordTargetMethodInvoke(command => command.CommandType = value);
                _commandType = value;
            }
        }

        private UpdateRowSource _updatedRowSource;
        public override UpdateRowSource UpdatedRowSource
        {
            get => _updatedRowSource;
            set
            {
                RecordTargetMethodInvoke(command => command.UpdatedRowSource = value);
                _updatedRowSource = value;
            }
        }

        /// <summary>
        /// 当前命令的链接
        /// </summary>
        protected override DbConnection DbConnection
        {
            get=>_shardingConnection;
            set
            {
                if (value is ShardingConnection shardingConnection)
                {
                    _shardingConnection = shardingConnection;
                }
                else
                {

                    throw new ShardingInvalidOperationException(
                        $"value is not {nameof(ShardingConnection)} cant set  {nameof(DbConnection)}");
                }
            }
        }


        private ShardingParameterCollection _parameters;
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
        protected override DbTransaction DbTransaction  { get; set; }

        private bool _designTimeVisible;
        public override bool DesignTimeVisible
        {
            get => _designTimeVisible;
            set
            {
                RecordTargetMethodInvoke(command => command.DesignTimeVisible = value);
                _designTimeVisible = value;
            }
        }

        protected override DbParameter CreateDbParameter()
        {
            var shardingParameter = new ShardingParameter();
            return shardingParameter;
        }


        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            if (string.IsNullOrWhiteSpace(this.CommandText))
                throw new ShardingException("sql command text null or empty");

            DbDataReader result;
            try
            {      

                executionContext = Prepare(CommandText);
                // List<IStreamDataReader> dataReaders = _commandExecutor.ExecuteQuery();
                // IStreamDataReader mergedResult = MergeQuery(dataReaders);
                // result = new ShardingDataReader(_commandExecutor.DbDataReaders, mergedResult, this, executionContext);
              
                var dataReaders = _commandExecutor.ExecuteDbDataReader(false,executionContext);
                
                var mergedResult = MergeQuery(dataReaders);
                result = new ShardingDataReader(_commandExecutor.GetDataReaders(), mergedResult, executionContext);
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
            ShardingRuntimeContext runtimeContext = _shardingConnection.GetRuntimeContext();
            MergeEngine mergeEngine = new MergeEngine(runtimeContext.GetRule().ToRules(),
                runtimeContext.GetProperties(), runtimeContext.GetDatabaseType(), runtimeContext.GetMetaData().Schema);
            return mergeEngine.Merge(streamDataReaders, executionContext.GetSqlCommandContext());
        }

        private ExecutionContext Prepare(string sql)
        {
            _commandExecutor.Clear();
            
            ShardingRuntimeContext runtimeContext = _shardingConnection.GetRuntimeContext();
            
            BasePrepareEngine prepareEngine = HasAnyParams()
                ? (BasePrepareEngine)new PreparedQueryPrepareEngine(
                    runtimeContext.GetRule().ToRules(), runtimeContext.GetProperties(), runtimeContext.GetMetaData(),
                    runtimeContext.GetSqlParserEngine())
                : (BasePrepareEngine)new SimpleQueryPrepareEngine(
                    runtimeContext.GetRule().ToRules(), runtimeContext.GetProperties(), runtimeContext.GetMetaData(),
                    runtimeContext.GetSqlParserEngine());
            var parameterContext =
                new ParameterContext(_parameters?.GetParams().Select(o => (DbParameter)o).ToArray() ??
                                     Array.Empty<DbParameter>());
            
            ExecutionContext result = prepareEngine.Prepare(sql, parameterContext);
            //TODO
            // _commandExecutor.Init(result);
            // //_commandExecutor.Commands.for
            // _commandExecutor.Commands.ForEach(ReplyTargetMethodInvoke);
            return result;
        }

        private bool HasAnyParams()
        {
            return _parameters != null && _parameters.GetParams().Any();
        }

        public event Action<DbCommand> OnRecorder;

        public void ReplyTargetMethodInvoke(DbCommand target)
        {
            OnRecorder?.Invoke(target);
        }

        public void RecordTargetMethodInvoke(Action<DbCommand> targetMethod)
        {
            OnRecorder += targetMethod;
        }

    }
}