using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ShardingConnector.AdoNet.AdoNet.Abstraction;
using ShardingConnector.AdoNet.AdoNet.Core.Command;
using ShardingConnector.AdoNet.AdoNet.Core.Transaction;
using ShardingConnector.Exceptions;
using ShardingConnector.NewConnector.DataSource;
using ShardingConnector.Transaction;
using ShardingRuntimeContext = ShardingConnector.AdoNet.AdoNet.Core.Context.ShardingRuntimeContext;

namespace ShardingConnector.AdoNet.AdoNet.Core.Connection
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/19 13:56:01
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class ShardingConnection : AbstractDbConnection
    {
        private readonly IDictionary<string, IDataSource> _dataSourceMap;
        private readonly ShardingRuntimeContext _runtimeContext;
        private readonly TransactionTypeEnum _transactionType;
        private bool _isOpenTransaction = false;
        private readonly DbConnection _defaultDbConnection;
        internal ShardingTransaction CurrentTransaction;

        public ShardingConnection(IDictionary<string, IDataSource> dataSourceMap, ShardingRuntimeContext runtimeContext,
            TransactionTypeEnum transactionType):this(
            dataSourceMap,
            runtimeContext,
            transactionType,
            dataSourceMap.Values.FirstOrDefault(o => o.IsDefault())?.CreateConnection() ??throw new ShardingException("not found default data source"))
        {
        }
        public ShardingConnection(IDictionary<string, IDataSource> dataSourceMap, ShardingRuntimeContext runtimeContext,
            TransactionTypeEnum transactionType,DbConnection defaultDbConnection)
        {
            _dataSourceMap = dataSourceMap;
            _runtimeContext = runtimeContext;
            _transactionType = transactionType;
            _defaultDbConnection =defaultDbConnection;
        }

        public override string ConnectionString
        {
            get => _defaultDbConnection.ConnectionString;
            set=>_defaultDbConnection.ConnectionString = value;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            var transaction = _defaultDbConnection.BeginTransaction(isolationLevel);
            RecordTargetMethodInvoke(connection => connection.BeginTransaction(isolationLevel));
            if (CachedConnectionsNotEmpty())
            {
                //RecordConnectionMethodInvoke(connection=>connection.BeginTransaction(isolationLevel));
                var multiTasks = CachedConnections.Values.SelectMany(o => o)
                    .Select(connection => Task.Run(() => connection.BeginTransaction(isolationLevel))).ToArray();
                Task.WaitAll(multiTasks);
            }

            var shardingTransaction = new ShardingTransaction(transaction,this);
            CurrentTransaction = shardingTransaction;
            return shardingTransaction;
        }

        public override void ChangeDatabase(string databaseName)
        {
            _defaultDbConnection.ChangeDatabase(databaseName);
            RecordTargetMethodInvoke(connection => connection.ChangeDatabase(databaseName));
            if (CachedConnectionsNotEmpty())
            {
                //RecordConnectionMethodInvoke(connection => connection.ChangeDatabase(databaseName));
                var multiTasks = CachedConnections.Values.SelectMany(o => o)
                    .Select(connection => Task.Run(() => connection.ChangeDatabase(databaseName))).ToArray();
                Task.WaitAll(multiTasks);
            }
        }

        public override void Close()
        {
            _defaultDbConnection.Close();
            if (CachedConnectionsNotEmpty())
            {
                var multiTasks = CachedConnections.Values.SelectMany(o => o)
                    .Select(connection => Task.Run(() => connection.Close())).ToArray();
                Task.WaitAll(multiTasks);
            }
        }

        public override void Open()
        {
            _defaultDbConnection.Open();
            RecordTargetMethodInvoke(connection => connection.Open());
            if (CachedConnectionsNotEmpty())
            {
                //RecordConnectionMethodInvoke(connection => connection.Open());
                var multiTasks = CachedConnections.Values.SelectMany(o => o)
                    .Select(connection => Task.Run(() => connection.Open())).ToArray();
                Task.WaitAll(multiTasks);
            }
        }

        public override string Database => _defaultDbConnection.Database;
        public override ConnectionState State => _defaultDbConnection.State;
        public override string DataSource => _defaultDbConnection.DataSource;
        public override string ServerVersion => _defaultDbConnection.ServerVersion;

        protected override DbCommand CreateDbCommand()
        {
            return new ShardingCommand(null, this);
        }

        public ShardingRuntimeContext GetRuntimeContext()
        {
            return _runtimeContext;
        }

        public bool IsHoldTransaction()
        {
            return (TransactionTypeEnum.LOCAL == _transactionType && _isOpenTransaction) ||
                   (TransactionTypeEnum.XA == _transactionType && IsInShardingTransaction());
        }

        private bool IsInShardingTransaction()
        {
            //return null != shardingTransactionManager && shardingTransactionManager.isInTransaction();
            return false;
        }

        public override IDictionary<string, IDataSource> GetDataSourceMap()
        {
            return _dataSourceMap;
        }

        public override DbConnection CreateConnection(string dataSourceName, IDataSource dataSource)
        {
            return dataSource.CreateConnection();
        }

        private bool _isDispose;

        protected override void Dispose(bool disposing)
        {
            _isDispose = true;
            _defaultDbConnection.Dispose();
            base.Dispose(disposing);
        }

        public override DbConnection GetDefaultDbConnection()
        {
            return _defaultDbConnection;
        }
        
        private void VerifyNotDisposed()
        {
            if (_isDispose)
                throw new ObjectDisposedException(GetType().Name);
        }
    }
}