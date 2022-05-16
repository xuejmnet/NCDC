using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using ShardingConnector.AdoNet.AdoNet.Abstraction;
using ShardingConnector.AdoNet.AdoNet.Core.Command;
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
    public class ShardingConnection: AbstractDbConnection
    {
        private readonly IDictionary<string, IDataSource> _dataSourceMap;
        private readonly ShardingRuntimeContext _runtimeContext;
        private readonly TransactionTypeEnum _transactionType;
        private bool isOpenTransaction = false;

        public ShardingConnection(IDictionary<string, IDataSource> dataSourceMap,ShardingRuntimeContext runtimeContext, TransactionTypeEnum transactionType)
        {
            _dataSourceMap = dataSourceMap;
            _runtimeContext = runtimeContext;
            _transactionType = transactionType;
        }
        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            isOpenTransaction = true;
            throw new NotImplementedException();
        }

        public override void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override void Open()
        {
            throw new NotImplementedException();
        }

        public override string ConnectionString { get; set; }
        public override string Database { get; }
        public override ConnectionState State { get; }
        public override string DataSource { get; }
        public override string ServerVersion { get; }

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
            return (TransactionTypeEnum.LOCAL == _transactionType && isOpenTransaction) || (TransactionTypeEnum.XA == _transactionType && IsInShardingTransaction());
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
    }
}
