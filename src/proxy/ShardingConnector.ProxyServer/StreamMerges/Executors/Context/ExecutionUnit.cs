namespace ShardingConnector.ProxyServer.StreamMerges.Executors.Context
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 23 March 2021 21:30:09
* @Email: 326308290@qq.com
*/
    public sealed class ExecutionUnit
    {
        private readonly string _dataSourceName;
        private readonly SqlUnit _sqlUnit;

        public ExecutionUnit(string dataSourceName, SqlUnit sqlUnit)
        {
            _dataSourceName = dataSourceName;
            _sqlUnit = sqlUnit;
        }

        public string GetDataSourceName()
        {
            return _dataSourceName;
        }

        private bool Equals(ExecutionUnit other)
        {
            return _dataSourceName == other._dataSourceName && Equals(_sqlUnit, other._sqlUnit);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is ExecutionUnit other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_dataSourceName != null ? _dataSourceName.GetHashCode() : 0) * 397) ^ (_sqlUnit != null ? _sqlUnit.GetHashCode() : 0);
            }
        }

        public SqlUnit GetSqlUnit()
        {
            return _sqlUnit;
        }
    }
}