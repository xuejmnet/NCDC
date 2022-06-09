using System;
using System.Data;
using System.Data.Common;
using ShardingConnector.AdoNet.AdoNet.Abstraction;
using ShardingConnector.AdoNet.AdoNet.Core.Connection;

namespace ShardingConnector.AdoNet.AdoNet.Core.Transaction
{
    public class ShardingTransaction:DbTransaction,IAdoMethodRecorder<DbTransaction>
    {
        private readonly DbTransaction _transaction;
        private readonly ShardingConnection _shardingConnection;

        public ShardingTransaction(DbTransaction transaction,ShardingConnection shardingConnection)
        {
            _transaction = transaction;
            _shardingConnection = shardingConnection;
        }
        public override void Commit()
        {
            _transaction.Commit();
            RecordTargetMethodInvoke(transaction => transaction.Commit());
        }

        public override void Rollback()
        {
            _transaction.Rollback();
            RecordTargetMethodInvoke(transaction => transaction.Rollback());
        }

        protected override DbConnection DbConnection => _shardingConnection;
        public override IsolationLevel IsolationLevel => _transaction.IsolationLevel;
        private event Action<DbTransaction> OnRecorder;
        public void ReplyTargetMethodInvoke(DbTransaction target)
        {
            OnRecorder?.Invoke(target);
        }

        public void RecordTargetMethodInvoke(Action<DbTransaction> targetMethod)
        {
            OnRecorder += targetMethod;
        }
    }
}