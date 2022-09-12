using OpenConnector.ShardingAdoNet;

namespace NCDC.ProxyServer.StreamMerges.Executors.Context
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 23 March 2021 21:30:29
* @Email: 326308290@qq.com
*/
    public sealed class SqlUnit
    {
        private readonly string _sql;
        private readonly ParameterContext _parameterContext;

        public SqlUnit(string sql,ParameterContext parameterContext)
        {
            _sql = sql;
            _parameterContext = parameterContext;
        }

        public string GetSql()
        {
            return _sql;
        }

        public ParameterContext GetParameterContext()
        {
            return _parameterContext;
        }
    }
}