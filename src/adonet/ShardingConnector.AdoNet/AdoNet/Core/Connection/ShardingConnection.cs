using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ShardingConnector.AdoNet.AdoNet.Abstraction;
using ShardingConnector.AdoNet.AdoNet.Core.Command;
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

        public ShardingConnection(IDictionary<string, IDataSource> dataSourceMap, ShardingRuntimeContext runtimeContext,
            TransactionTypeEnum transactionType)
        {
            _dataSourceMap = dataSourceMap;
            _runtimeContext = runtimeContext;
            _transactionType = transactionType;
            _defaultDbConnection = dataSourceMap.Values.FirstOrDefault(o => o.IsDefault())?.CreateConnection() ??
                                   throw new ShardingException("not found default data source");
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            _isOpenTransaction = true;
            var transaction = _defaultDbConnection.BeginTransaction(isolationLevel);
            RecordTargetMethodInvoke(connection => connection.BeginTransaction(isolationLevel));
            if (_runtimeContext.IsMultiDataSource())
            {
                //RecordConnectionMethodInvoke(connection=>connection.BeginTransaction(isolationLevel));
                var multiTasks = CachedConnections.Values.SelectMany(o => o)
                    .Select(connection => Task.Run(() => connection.BeginTransaction(isolationLevel))).ToArray();
                Task.WaitAll(multiTasks);
            }

            return transaction;
        }

        public override void ChangeDatabase(string databaseName)
        {
            _defaultDbConnection.ChangeDatabase(databaseName);
            RecordTargetMethodInvoke(connection => connection.ChangeDatabase(databaseName));
            if (_runtimeContext.IsMultiDataSource())
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
            if (_runtimeContext.IsMultiDataSource())
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
            if (_runtimeContext.IsMultiDataSource())
            {
                //RecordConnectionMethodInvoke(connection => connection.Open());
                var multiTasks = CachedConnections.Values.SelectMany(o => o)
                    .Select(connection => Task.Run(() => connection.Open())).ToArray();
                Task.WaitAll(multiTasks);
            }
        }

        public override string ConnectionString
        {
            get { return _defaultDbConnection.ConnectionString; }
            set { _defaultDbConnection.ConnectionString = value; }
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

        /// <summary>
        /// 获取当前connection的默认链接
        /// </summary>
        /// <returns></returns>
        public override DbConnection GetDefaultDbConnection()
        {
            return _defaultDbConnection;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _defaultDbConnection.Dispose();
        }
    }
}