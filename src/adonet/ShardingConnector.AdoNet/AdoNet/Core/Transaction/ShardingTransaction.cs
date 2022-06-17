using System;
using System.Data;
using System.Data.Common;
using ShardingConnector.AdoNet.AdoNet.Abstraction;
using ShardingConnector.AdoNet.AdoNet.Core.Connection;

namespace ShardingConnector.AdoNet.AdoNet.Core.Transaction
{
    public class ShardingTransaction : DbTransaction, IAdoMethodRecorder<DbTransaction>
    {
        private readonly DbTransaction _transaction;

        public ShardingTransaction(DbTransaction transaction, ShardingConnection shardingConnection)
        {
            _transaction = transaction;
            Connection = shardingConnection;
        }

        public override void Commit()
        {
            VerifyValid();
            _transaction.Commit();
            RecordTargetMethodInvoke(transaction => transaction.Commit());
        }

        public override void Rollback()
        {
            VerifyValid();
            _transaction.Rollback();
            RecordTargetMethodInvoke(transaction => transaction.Rollback());
        }

        public new ShardingConnection Connection { get; private set; }
        protected override DbConnection DbConnection => Connection;
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

        private bool _isDispose = false;

        protected override void Dispose(bool disposing)
        {
            _isDispose = true;
            DoDispose();
        }

        private void DoDispose()
        {
            _transaction.Dispose();
            if (Connection?.CurrentTransaction == this)
            {
                Connection.CurrentTransaction = null;
            }

            Connection = null;
        }

        private void VerifyValid()
        {
            if (_isDispose)
                throw new ObjectDisposedException(nameof(ShardingTransaction));
            if (Connection is null)
                throw new InvalidOperationException("Already committed or rolled back.");
            if (Connection.CurrentTransaction is null)
                throw new InvalidOperationException("There is no active transaction.");
            if (!object.ReferenceEquals(Connection.CurrentTransaction, this))
                throw new InvalidOperationException("This is not the active transaction.");
        }
    }
}