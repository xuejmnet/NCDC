namespace ShardingConnector.Kernels.Parse
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

        public SqlUnit GetSqlUnit()
        {
            return _sqlUnit;
        }
    }
}