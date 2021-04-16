using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using ShardingConnector.ShardingAdoNet.AdoNet.Core.Context;
using ShardingConnector.Transaction;
using ShardingConnector.Transaction.Spi;

namespace ShardingConnector.ShardingAdoNet
{
/*
* @Author: xjm
* @Description:
* @Date: Saturday, 20 March 2021 16:02:53
* @Email: 326308290@qq.com
*/
    public class ShardingConnection:DbConnection
    {
        private readonly IDictionary<string, DbProviderFactory> _dbProviderFactories;
        private bool isOpened = false;
        private readonly ShardingRuntimeContext _runtimeContext;
        private readonly TransactionTypeEnum _transactionType;
        private readonly IShardingTransactionManager _transactionManager;

        public ShardingConnection(IDictionary<string, DbProviderFactory> dbProviderFactories,ShardingRuntimeContext runtimeContext,TransactionTypeEnum transactionType)
        {
            _dbProviderFactories = dbProviderFactories;
            _runtimeContext = runtimeContext;
            _transactionType = transactionType;
        }
        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
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
            this.isOpened = true;
        }

        public override string ConnectionString { get; set; }
        public override string Database { get; }
        public override ConnectionState State { get; }
        public override string DataSource { get; }
        public override string ServerVersion { get; }

        protected override DbCommand CreateDbCommand()
        {
            //d
            return new ShardingCommand(null, this);
        }
    }
}