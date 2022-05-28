using System.Data;
using System.Data.Common;

namespace ShardingConnector.Transaction;

public class ShardingTransaction:DbTransaction
{
    public override void Commit()
    {
        throw new System.NotImplementedException();
    }

    public override void Rollback()
    {
        throw new System.NotImplementedException();
    }

    protected override DbConnection DbConnection { get; }
    public override IsolationLevel IsolationLevel { get; }
}